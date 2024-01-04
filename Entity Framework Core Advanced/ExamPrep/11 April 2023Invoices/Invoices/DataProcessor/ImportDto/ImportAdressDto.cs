
using Invoices.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto;


[XmlType("Address")]
public class ImportAdressDto
{

    [XmlElement("StreetName")]
    [Required]
    [MaxLength(Validations.AdresStreetNameMaxlength)]
    [MinLength(Validations.AdressStreetNAMEmINlENGHT)]
    public string StreetName { get; set; }


    [XmlElement("StreetNumber")]

    public int StreetNumber { get; set; }


    [XmlElement("PostCode")]
    [Required]
    public string PostCode { get; set; } 


    [XmlElement("City")]
    [Required]
    [MaxLength(Validations.CityMaxLengt)]
    [MinLength(Validations.CityMINLengt)]
    public string City { get; set; } 


    [XmlElement("Country")]
    [Required]
    [MaxLength(Validations.CityMaxLengt)]
    [MinLength(Validations.CityMINLengt)]
    public string Country { get; set; } 

}
