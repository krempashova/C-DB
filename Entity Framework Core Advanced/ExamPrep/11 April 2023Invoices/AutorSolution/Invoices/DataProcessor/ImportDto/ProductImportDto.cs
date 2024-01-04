using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Invoices.Shared.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto
{
    public class ProductImportDto
    {
        [Required]
        [MaxLength(ProductNameMaxLength)]
        [MinLength(ProductNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range((double)ProductPriceMinValue, (double)ProductPriceMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0,4)]
        public CategoryType CategoryType { get; set; }

        public int[] Clients { get; set; } = null!;
    }
}
