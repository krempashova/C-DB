using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Shared.GlobalConstants;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ClientImportDto
    {
        [Required]
        [XmlElement("Name")]
        [MaxLength(ClientNameMaxLength)]
        [MinLength(ClientNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("NumberVat")]
        [MaxLength(ClientNumberVatMaxLength)]
        [MinLength(ClientNumberVatMinLength)]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        public AddressImportDto[] Addresses { get; set; } = null!;
    }
}
