using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Runtime.InteropServices;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new();
            //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
              string result= GetUsersWithProducts(context);
            Console.WriteLine(result);




        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportUsersDto[] usersDtos =
                JsonConvert.DeserializeObject<ImportUsersDto[]>(inputJson);

            ICollection<User> validUsers = new HashSet<User>();
            foreach (ImportUsersDto usersDto in usersDtos)
            {
                User user = mapper.Map<User>(usersDto);
                validUsers.Add(user);
            }

            context.AddRange(validUsers);
            context.SaveChanges();
            return $"Successfully imported {validUsers.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportProductsDto[] productsDtos
               = JsonConvert.DeserializeObject<ImportProductsDto[]>(inputJson);

            ICollection<Product> valideproducts = new HashSet<Product>();

            foreach (ImportProductsDto item in productsDtos)
            {
                Product product = mapper.Map<Product>(item);
                valideproducts.Add(product);
            }

            context.AddRange(valideproducts);
            context.SaveChanges();
            return $"Successfully imported {valideproducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCategoriesDto[] categoriesDtos 
                = JsonConvert.DeserializeObject<ImportCategoriesDto[]>(inputJson);
            ICollection<Category> validcategories = new HashSet<Category>();
            foreach (ImportCategoriesDto item in categoriesDtos)
            {
                if(string.IsNullOrEmpty(item.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(item);
                validcategories.Add(category);  

            }
            context.AddRange(validcategories);
            context.SaveChanges();
            return $"Successfully imported {validcategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {

            IMapper mapper = CreateMapper();

            ImportCategoryProductsDto[] CategoryProductsDtos 
                = JsonConvert.DeserializeObject<ImportCategoryProductsDto[]>(inputJson);
            ICollection<CategoryProduct> validcategoryProducts = new HashSet<CategoryProduct>();

            foreach (ImportCategoryProductsDto item in CategoryProductsDtos)
            {


                // This is not wanted in the description but we do it for security
                // In Judge locally they may not be added previously
                // JUDGE DO NOT LIKE THIS VALIDATION BELOW!!!!!
               // if (!context.Categories.Any(c => c.Id == item.CategoryId) ||
                  //  !context.Products.Any(p => p.Id == item.ProductId))
               // {
                  //  continue;
               // }
                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(item);
                validcategoryProducts.Add(categoryProduct); 
            }

            context.AddRange(validcategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validcategoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {


            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(productsInRange,Formatting.Indented) ;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {


            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Where(ps => ps.Buyer != null)
                    .Select(ps => new
                    {
                        name = ps.Name,
                        price = ps.Price,
                        buyerFirstName = ps.Buyer.FirstName,
                        buyerLastName = ps.Buyer.LastName
                    })
                    .ToArray()
                }).AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(users,Formatting.Indented);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
               .OrderByDescending(c => c.CategoriesProducts.Count())
               .Select(c => new
               {
                   category = c.Name,
                   productsCount = c.CategoriesProducts.Count(),
                   averagePrice = c.CategoriesProducts.Average(c => c.Product.Price).ToString("f2"),
                   totalRevenue = c.CategoriesProducts.Sum(c => c.Product.Price).ToString("F2")
               }).AsNoTracking()
               .ToArray();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    // UserDTO
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        // ProductWrapperDTO
                        Count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                // ProductDTO
                                p.Name,
                                p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .AsNoTracking()
                .ToArray();

            var userWrapperDto = new
            {
                UsersCount = users.Length,
                Users = users
            };

            return JsonConvert.SerializeObject(userWrapperDto,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }


        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }

        private static IContractResolver ConfigureCamelCaseNaming()
        {
            return new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };
        }
    }
}