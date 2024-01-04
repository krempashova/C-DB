
using Cadastre.Common;
using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos;

[XmlType("District")]
public class ImportDistrictDto
{
    // maybe to change after!!!!
    [XmlAttribute("Region")]
    [Required]
    [Range(0,3)]
    public Region Region { get; set; }


    [XmlElement("Name")]
    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string Name { get; set; } = null!;

    [XmlElement("PostalCode")]
    [Required]
    [MinLength(8)]
    [MaxLength(8)]
    [RegularExpression(Validation.RegexPostalCode)]
    public string PostalCode { get; set; } = null!;

    [XmlArray("Properties")]
    public ImportPropertyDto[] Properties { get; set; } = null!;


}
