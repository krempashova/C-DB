
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto;


[XmlType("Despatcher")]
public class ImportDespacherDto
{
    [XmlElement("Name")]
    [Required]
    [MaxLength(ValidationConstants.DespecherNameMaxLenght)]
    [MinLength(ValidationConstants.DespacherMinLenght)]
    public string Name { get; set; } = null!;

    [XmlElement("Position")]
    [Required]
    public string Position { get; set; } = null!;


    [XmlArray("Trucks")]
    public ImportTruckDto[] Truks { get; set; } = null!;

}
