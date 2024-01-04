
using Invoices.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto;


[XmlType("Client")]
public class ImportClientDto
{

    [XmlElement("Name")]
    [Required]
    [MaxLength(Validations.ClientNameMaxLengt)]
    [MinLength(Validations.ClientNameMinLength)]
    public string Name { get; set; } = null!;


    [XmlElement("NumberVat")]
    [Required]
    [MaxLength(Validations.ClientnumberVatmaxlenght)]
    [MinLength(Validations.ClientNumberVatMinLenght)]
    public string NumberVat { get; set; } = null!;


    [XmlArray("Addresses")]
    [Required]
    public ImportAdressDto[] Adresses { get; set; } = null!;


}
