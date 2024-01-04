
using Medicines.Common;
using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos;


[XmlType("Medicine ")]
public class ImportMedicineDto
{

    [XmlAttribute("category")]
    [Required]
    [Range(ValidationConstants.CATEGORYMIN,ValidationConstants.categoryMax)]
    public Category Category { get; set; }

    [XmlElement("Name")]
    [Required]
    [MinLength(ValidationConstants.MedicineNameMin)]
    [MaxLength(ValidationConstants.MedicinenameMaxlenght)]
    public string Name { get; set; } = null!;

    [XmlElement("Price")]
    [Required]
    [Range(ValidationConstants.PriceMin,ValidationConstants.PriceMax)]
    public decimal Price { get; set; }


    [XmlElement("ProductionDate")]
    [Required]
    public DateTime ProductionDate { get; set; } 


    [XmlElement("ExpiryDate")]
    [Required]
    public DateTime ExpiryDate { get; set; } 

    [XmlElement("Producer")]
    [MinLength(ValidationConstants.ProsucerMiN)]
    [MaxLength(ValidationConstants.ProducerMAxLENGHT)]
    public string? Producer { get; set; }

}
