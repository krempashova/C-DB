
using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto;


[XmlType("Footballer")]
public class ExportFootballerDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public PositionType PositionType { get; set; }

}
