using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using static Invoices.Shared.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto
{
    public class InvoiceImportDto
    {
        [Required]
        [Range(InvoiceNumberMinValue, InvoiceNumberMaxValue)]
        public int Number { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Range(0,2)]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
