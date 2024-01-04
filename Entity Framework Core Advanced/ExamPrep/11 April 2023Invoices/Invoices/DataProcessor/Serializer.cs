namespace Invoices.DataProcessor
{
    using AutoMapper.QueryableExtensions;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {

           

            XmlHelper xmlHelper = new XmlHelper();

            ExportClientDto[] clientsInvoices =
                context.Clients
                .Where(c => c.Invoices.Any(ci => ci.IssueDate > date))
               .Select(c => new ExportClientDto()
               {
                   ClientName = c.Name,
                   VatNumber=c.NumberVat,
                   InvoicesCount=c.Invoices.Count,
                   Invoices=c.Invoices
                   .OrderBy(i=>i.IssueDate)
                   .ThenByDescending(i=>i.DueDate)
                   .Select(i=>new ExportInvoiceDto()
                   {
                       InvoiceNumber = i.Number,
                       DueDate = i.DueDate.ToString("d"),
                       Currency = i.CurrencyType.ToString(),
                       InvoiceAmount = decimal.Parse(i.Amount.ToString("0.##"))

                   })
                   
                     .ToArray()

               })
               .OrderByDescending(c=>c.InvoicesCount)
               .ThenBy(c=>c.ClientName)
               .ToArray();


            return xmlHelper.Serialize(clientsInvoices, "Clients");



             






         }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

           var products=context.Products
                .Where(p=>p.ProductsClients.Any(pc=>pc.Client.Name.Length>=nameLength))
                .ToArray()
                .Select(p => new
                {
                    Name=p.Name,
                    Price=p.Price,
                    Category= p.CategoryType.ToString(),
                    Clients=p.ProductsClients
                    .Where(pc=>pc.Client.Name.Length>=nameLength)
                    .ToArray()
                    .OrderBy(pc=>pc.Client.Name)
                    .Select(pc => new
                    {
                        Name=pc.Client.Name,
                        NumberVat=pc.Client.NumberVat,
                     
                    })
                    .ToArray()
                })
                 .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}