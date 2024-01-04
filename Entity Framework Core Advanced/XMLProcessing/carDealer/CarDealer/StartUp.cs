using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
       // string inputXml = File.ReadAllText(@"../../../Datasets/sales.xml");
        string result= GetSalesWithAppliedDiscount(context);
        Console.WriteLine(result);
    }

    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportSupplierDto[] supplierDtos =
            xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

        ICollection<Supplier> suppliers = new HashSet<Supplier>();
        foreach (var item in supplierDtos)
        {

            if (string.IsNullOrEmpty(item.Name))
            {
                continue;
            }
            Supplier supplier = mapper.Map<Supplier>(item);
            suppliers.Add(supplier);
        }

        context.Suppliers.AddRange
            (suppliers);
        context.SaveChanges();
        return $"Successfully imported {suppliers.Count}";

    }

    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportPartDto[] partDtos =
            xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

        ICollection<Part> validParts = new HashSet<Part>();
        foreach (ImportPartDto partDto in partDtos)
        {
            if (string.IsNullOrEmpty(partDto.Name))
            {
                continue;
            }

            if (!partDto.SupplierId.HasValue ||
                !context.Suppliers.Any(s => s.Id == partDto.SupplierId))
            {
                // Missing or wrong supplier id
                continue;
            }

            Part part = mapper.Map<Part>(partDto);
            validParts.Add(part);
        }

        context.Parts.AddRange(validParts);
        context.SaveChanges();

        return $"Successfully imported {validParts.Count}";
    }

    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportCarDto[] carDtos =
            xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

        ICollection<Car> validCars = new HashSet<Car>();
        foreach (ImportCarDto carDto in carDtos)
        {
            if (string.IsNullOrEmpty(carDto.Make) ||
                string.IsNullOrEmpty(carDto.Model))
            {
                continue;
            }

            Car car = mapper.Map<Car>(carDto);

            foreach (var partDto in carDto.Parts.DistinctBy(p => p.PartId))
            {
                if (!context.Parts.Any(p => p.Id == partDto.PartId))
                {
                    continue;
                }

                PartCar carPart = new PartCar()
                {
                    PartId = partDto.PartId
                };
                car.PartsCars.Add(carPart);
            }

            validCars.Add(car);
        }

        context.Cars.AddRange(validCars);
        context.SaveChanges();

        return $"Successfully imported {validCars.Count}";


    }

    public static string ImportCustomers(CarDealerContext context, string inputXml)
    {

        XmlHelper xmlHelper = new XmlHelper();
        IMapper mapper = InitializeAutoMapper();

        ImportCustomerDto[] customerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");
        ICollection<Customer> validcustomers = new HashSet<Customer>();
        foreach (ImportCustomerDto customerDto in customerDtos)
        {
            if(string.IsNullOrEmpty(customerDto.Name))
            {
                continue;
            }
            Customer customer = mapper.Map<Customer>(customerDto);
            validcustomers.Add(customer);
        }

        context.AddRange(validcustomers);
        context.SaveChanges();
        return $"Successfully imported {validcustomers.Count}";
    }

    public static string ImportSales(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportSaleDto[] SaleDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

        ICollection<Sale> validsales = new HashSet<Sale>();
        foreach (ImportSaleDto SaleDto in SaleDtos)
        {
            if(!SaleDto.CarId.HasValue
                || !context.Cars.Any(c=>c.Id==SaleDto.CarId.Value))
            {
                continue;
            }
            Sale sale = mapper.Map<Sale>(SaleDto);
            validsales.Add(sale);
        }

        context.AddRange(validsales);
        context.SaveChanges();
        return $"Successfully imported {validsales.Count}";
    }

    public static string GetCarsWithDistance(CarDealerContext context)
    {
        IMapper mapper = InitializeAutoMapper();    
        XmlHelper xmlHelper = new XmlHelper();
        ExportCarDto[] cars = context.Cars
               .Where(c => c.TraveledDistance > 2000000)
               .OrderBy(c => c.Make)
               .ThenBy(c => c.Model)
               .Take(10)
               .ProjectTo<ExportCarDto>(mapper.ConfigurationProvider)
               .ToArray();

        return xmlHelper.Serialize<ExportCarDto[]>(cars, "cars");
    }

    public static string GetCarsFromMakeBmw(CarDealerContext context)
    {
        IMapper mapper = InitializeAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ExportBmwCarDto[] bmwCarDtos = context.Cars
            .Where(c => c.Make == "BMW")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .ProjectTo<ExportBmwCarDto>(mapper.ConfigurationProvider)
            .ToArray();
        return xmlHelper.Serialize<ExportBmwCarDto[]>(bmwCarDtos, "cars");
    }

    public static string GetLocalSuppliers(CarDealerContext context)
    {
        IMapper mapper = InitializeAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ExportSupplierDto[] supplierDtos = context.Suppliers
            .Where(s => s.IsImporter == false)
            .ProjectTo<ExportSupplierDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize<ExportSupplierDto[]>(supplierDtos, "suppliers");
    }

    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        IMapper mapper = InitializeAutoMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ExportCarWithPartsDto[] carsWithParts = context
            .Cars
            .OrderByDescending(c => c.TraveledDistance)
            .ThenBy(c => c.Model)
            .Take(5)
            .ProjectTo<ExportCarWithPartsDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize(carsWithParts, "cars");
    }

    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {

        var temp = context.Customers
            .Where(c => c.Sales.Any())
            .Select(c => new
            {
                FullName = c.Name,
                BoughtCars = c.Sales.Count(),
                SalesInfo = c.Sales.Select(s => new
                {
                    Prices = c.IsYoungDriver
                            ? s.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price * 0.95, 2))
                            : s.Car.PartsCars.Sum(pc => (double)pc.Part.Price)
                })
                    .ToArray(),
            })
            .ToArray();

        ExportCustomerDto[] totalSales = temp.OrderByDescending(x => x.SalesInfo.Sum(y => y.Prices))
                .Select(x => new ExportCustomerDto()
                {
                    FullName = x.FullName,
                    BoughtCars = x.BoughtCars,
                    SpentMoney = x.SalesInfo.Sum(y => (decimal)y.Prices)
                })
                .ToArray();

        XmlHelper xmlHelper = new XmlHelper();
        return xmlHelper.Serialize<ExportCustomerDto[]>(totalSales, "customers");
    }


    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {

        XmlHelper xmlHelper = new XmlHelper();

        ExportSaleAppliedDiscountDto[] sales = context.Sales
                .Select(s => new ExportSaleAppliedDiscountDto()
                {
                    Car = new ExportCarWithAttrDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = (int)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(p => p.Part.Price),
                    PriceWithDiscount =
                        Math.Round((double)(s.Car.PartsCars
                            .Sum(p => p.Part.Price) * (1 - (s.Discount / 100))), 4)
                })
                .ToArray();

        return xmlHelper.Serialize<ExportSaleAppliedDiscountDto[]>(sales, "sales");
    }

    private static IMapper InitializeAutoMapper()
      => new Mapper(new MapperConfiguration(cfg =>
      {
          cfg.AddProfile<CarDealerProfile>();
      }));

}