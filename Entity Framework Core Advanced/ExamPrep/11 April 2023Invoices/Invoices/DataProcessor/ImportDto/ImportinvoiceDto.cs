
using Invoices.Common;
using Invoices.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto;

public class ImportinvoiceDto
{

    [JsonProperty("Number")]
    [Required]
    [Range(Validations.InvoiceNumberMin,Validations.InvoiceNumberMax)]
    public int Number { get; set; }



    [JsonProperty("IssueDate")]
    [Required]
    public DateTime IssueDate { get; set; }


    [JsonProperty("DueDate")]
    [Required]
    public DateTime DueDate { get; set; } 


    [JsonProperty("Amount")]
    [Required]
    public decimal Amount { get; set; }


    [JsonProperty("CurrencyType")]
    [Required]
    [Range(Validations.CurrencyTypeMin,Validations.CurencyTypeMax)]
    public int CurrencyType { get; set; }


    [JsonProperty("ClientId")]
    [Required]
    public int ClientId { get; set; }

}
