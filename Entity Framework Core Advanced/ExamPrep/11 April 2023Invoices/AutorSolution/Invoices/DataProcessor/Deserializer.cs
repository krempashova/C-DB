namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using System.Globalization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClientImportDto[]), new XmlRootAttribute("Clients"));

            using StringReader stringReader = new StringReader(xmlString);

            ClientImportDto[] clientsDto = (ClientImportDto[])xmlSerializer.Deserialize(stringReader);

            List<Client> clients = new List<Client>();

            foreach (ClientImportDto clientDto in clientsDto)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client c = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat
                };

                foreach (AddressImportDto addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address a = new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country
                    };

                    c.Addresses.Add(a);
                }
                clients.Add(c);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, c.Name));
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            InvoiceImportDto[] invoicesDtos = JsonConvert.DeserializeObject<InvoiceImportDto[]>(jsonString);

            List<Invoice> invoices = new List<Invoice>();

            foreach (InvoiceImportDto invoiceDto in invoicesDtos)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (invoiceDto.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) || invoiceDto.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice i = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = invoiceDto.IssueDate,
                    DueDate = invoiceDto.DueDate,
                    CurrencyType = invoiceDto.CurrencyType,
                    Amount = invoiceDto.Amount,
                    ClientId = invoiceDto.ClientId
                };

                if (i.IssueDate > i.DueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                invoices.Add(i);
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, i.Number));
            }
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {


            StringBuilder sb = new StringBuilder();
            ProductImportDto[] productDtos = JsonConvert.DeserializeObject<ProductImportDto[]>(jsonString);

            List<Product> products = new List<Product>();

            foreach (ProductImportDto productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product p = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = productDto.CategoryType,
                };

                foreach (int clientId in productDto.Clients.Distinct())
                {
                    Client c = context.Clients.Find(clientId);
                    if (c == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                   p.ProductsClients.Add(new ProductClient()
                    {
                        Client = c
                    });
                }
                products.Add(p);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, p.Name, p.ProductsClients.Count));
            }
            context.Products.AddRange(products);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }



        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
