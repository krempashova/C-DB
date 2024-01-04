using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Shared.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class AddressImportDto
    {
        [Required]
        [XmlElement("StreetName")]
        [MaxLength(AddressStreetNameMaxLength)]
        [MinLength(AddressStreetNameMinLength)]
        public string StreetName { get; set; } = null!;

        [Required]
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }

        [Required]
        [XmlElement("PostCode")]
        public string PostCode { get; set; } = null!;

        [Required]
        [MaxLength(AddressCityMaxLength)]
        [MinLength(AddressCityMinLength)]
        [XmlElement("City")]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(AddressCountryMaxLength)]
        [MinLength(AddressCountryMinLength)]
        [XmlElement("Country")]
        public string Country { get; set; } = null!;
    }
}
