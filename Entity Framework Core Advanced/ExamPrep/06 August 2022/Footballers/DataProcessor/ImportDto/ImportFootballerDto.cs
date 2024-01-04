
using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto;


[XmlType("Footballer")]
public class ImportFootballerDto
{

    [XmlElement("Name")]
    [Required]
    [MaxLength(40)]
    [MinLength(2)]
    public string Name { get; set; } = null!;


    [XmlElement("ContractStartDate")]
    [Required]
    public string ContractStartDate { get; set; } = null!;

    [XmlElement("ContractEndDate")]
    [Required]
    public string ContractEndDate { get; set; } = null!;


    [XmlElement("BestSkillType")]
    [Required]
    [Range(0,4)]
    public int BestSkillType { get; set; }


    [XmlElement("PositionType")]
    [Required]
    [Range(0,3)]
    public int PositionType { get; set; }




}
