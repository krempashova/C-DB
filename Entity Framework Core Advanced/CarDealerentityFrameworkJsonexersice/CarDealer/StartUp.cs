using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new();
            //string inputJson=File.ReadAllText(@"../../../Datasets/sales.json");
            string result = GetSalesWithAppliedDiscount(context);
            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportSuppliersDto[] supplierDtos 
                = JsonConvert.DeserializeObject<ImportSuppliersDto[]>(inputJson);
            ICollection<Supplier> validsuppliers = new HashSet<Supplier>();
            foreach (ImportSuppliersDto item in supplierDtos)
            {
                Supplier supplier = mapper.Map<Supplier>(item);
                validsuppliers.Add(supplier);
            }
            context.AddRange(validsuppliers);
            context.SaveChanges();

            return $"Successfully imported {validsuppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {

            IMapper mapper = CreateMapper();

            ImportPartsDto[] ImportPartDtos = JsonConvert.DeserializeObject<ImportPartsDto[]>(inputJson);

            

            ICollection<Part> validparts = new HashSet<Part>();
            foreach (ImportPartsDto item in ImportPartDtos)
                
            {
                if(!context.Suppliers.Any(c=>c.Id==item.SupplierId))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(item);
                validparts.Add(part);

            }

            context.AddRange(validparts);
            context.SaveChanges();
            return $"Successfully imported {validparts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {


            IMapper mapper = CreateMapper();
            ImportCarsDto[] carDtos = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);
            ICollection<Car> validcars = new HashSet<Car>();

            foreach (ImportCarsDto item in carDtos)
            {
                Car car = mapper.Map<Car>(item);

                foreach (var partId in item.PartsId)
                {
                    context.PartsCars.Add(new PartCar
                    {
                        PartId = partId,
                        Car = car
                    });

                }
                validcars.Add(car);
            }
            context.AddRange(validcars);
            context.SaveChanges();
            return $"Successfully imported {validcars.Count}.";

        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCustomersDto[] cusmomerDtos 
                = JsonConvert.DeserializeObject<ImportCustomersDto[]>(inputJson);
            ICollection<Customer> validcustomers = new HashSet<Customer>();

            foreach (ImportCustomersDto item in cusmomerDtos)
            {
                Customer customer = mapper.Map<Customer>(item);

                validcustomers.Add(customer);
            }

            context.AddRange(validcustomers);
            context.SaveChanges();

            return $"Successfully imported {validcustomers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {

            IMapper mapper = CreateMapper();

            ImportSalesDto[] saleDtos = JsonConvert.DeserializeObject<ImportSalesDto[]>(inputJson);

            ICollection<Sale> sales = new HashSet<Sale>();
            foreach (ImportSalesDto item in saleDtos)
            {
                Sale sale = mapper.Map<Sale>(item);
                sales.Add(sale);
            }
            context.AddRange(sales);
            context.SaveChanges();

              return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var orderedCustomers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenByDescending(c => c.BirthDate)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    c.IsYoungDriver

                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                })
                .AsNoTracking()
                .ToArray();
            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count()
                })
                .AsNoTracking()
                .ToArray();
            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars.Select(c => new
            {
                car = new
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                },
                parts = c.PartsCars.Select(pc => new
                {
                    Name = pc.Part.Name,
                    Price = pc.Part.Price.ToString("f2")
                }).ToList()

            }).ToList();



            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count() > 0)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();
            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {


            var sels = context.Sales

                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(pc => pc.Part.Price).ToString("F2"),
                    priceWithDiscount = ((s.Car.PartsCars.Sum(pc => pc.Part.Price)) * (1 - s.Discount * 0.01m))
                .ToString("f2")



                }).Take(10)
                .AsNoTracking()
                .ToArray();
            return JsonConvert.SerializeObject(sels, Formatting.Indented);
        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}