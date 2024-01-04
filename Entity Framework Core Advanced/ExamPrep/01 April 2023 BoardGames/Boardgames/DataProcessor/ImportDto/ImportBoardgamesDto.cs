
using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto;


[XmlType("Boardgame")]
public class ImportBoardgamesDto
{

    [XmlElement("Name")]
    [Required]
    [MaxLength(20)]
    [MinLength(10)]
    public string Name { get; set; } = null!;


    [XmlElement("Rating")]
    [Range(1.00,10.00)]
    [Required]
    public double Rating { get; set; }


    [XmlElement("YearPublished")]
    [Range(2018,2023)]
    [Required]
    public int YearPublished { get; set; }


    [XmlElement("CategoryType")]
    [Required]
    public int CategoryType { get; set; }

    [XmlElement("Mechanics")]
    [Required]
    public string Mechanics { get; set; } = null!;


}
