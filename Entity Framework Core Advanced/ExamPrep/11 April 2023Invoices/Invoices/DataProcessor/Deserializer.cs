namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;

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
            XmlHelper xmlHelper = new XmlHelper();

            ICollection<Client> validClients = new HashSet<Client>();
            ImportClientDto[] clientDtos =
                xmlHelper.Deserialize<ImportClientDto[]>(xmlString, "Clients");

            foreach (ImportClientDto clDto in clientDtos)
            {


                if (!IsValid(clDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client client = new Client()
                {
                    Name = clDto.Name,
                    NumberVat = clDto.NumberVat,

                };

                foreach (ImportAdressDto adres in clDto.Adresses)
                {
                    if (!IsValid(adres))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrEmpty(adres.StreetName))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (string.IsNullOrEmpty(adres.PostCode)
                        && string.IsNullOrEmpty(adres.City)
                        && string.IsNullOrEmpty(adres.Country))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address ad = new Address()
                    {
                        StreetName = adres.StreetName,
                        StreetNumber = adres.StreetNumber,
                        PostCode = adres.PostCode,
                        City = adres.City,
                        Country = adres.Country
                    };

                    client.Addresses.Add(ad);
                }

                validClients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, client.Name));
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportinvoiceDto[] invoiceDtos =
                JsonConvert.DeserializeObject<ImportinvoiceDto[]>(jsonString);

            ICollection<Invoice> validInvoices = new HashSet<Invoice>();

            foreach (ImportinvoiceDto inDto in invoiceDtos)
            {
                if(!IsValid(inDto))
                {

                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (inDto.DueDate <inDto.IssueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                Invoice invoice = new Invoice()
                    { 
                       Number= inDto.Number,
                       IssueDate=inDto.IssueDate,
                       DueDate = inDto.DueDate,
                       Amount=inDto.Amount,
                       CurrencyType=(CurrencyType)inDto.CurrencyType,
                       ClientId=inDto.ClientId
                };
                    

                validInvoices.Add(invoice);
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, invoice.Number));


            }

            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();
            return sb.ToString().TrimEnd();






        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {

            StringBuilder sb = new StringBuilder();

            ImportProductDto[] productDtos =
                JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);
            ICollection<Product> validproducts = new HashSet<Product>();

          

            foreach (ImportProductDto prDto in productDtos)
            {

                if(!IsValid(prDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product product = new Product()
                {
                    Name = prDto.Name,
                    Price = prDto.Price,
                    CategoryType = prDto.CategoryType,
                };
                foreach (int clientId in prDto.ClientIds.Distinct())
                {
                    Client cl = context.Clients.Find(clientId);

                    if(cl==null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    product.ProductsClients.Add(new ProductClient()
                    {
                        Client = cl

                    });

                    

                }
                validproducts.Add(product);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));

            }

            context.Products.AddRange(validproducts);
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
