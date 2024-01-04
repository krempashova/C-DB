

using Medicines.Common;
using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos;


[XmlType("Medicine")]
public class ImportMedicinesDto
{

    [XmlAttribute("category")]
    [Range(ValidationConstants.CATEGORYMIN,ValidationConstants.categoryMax)]
    public int Category { get; set; }



    [XmlElement("Name")]
    [Required]
    [MaxLength(ValidationConstants.MedicinenameMaxlenght)]
    [MinLength(ValidationConstants.MedicineNameMin)]
    public string Name { get; set; } = null!;


    [XmlElement("Price")]
    [Range(ValidationConstants.PriceMin,ValidationConstants.PriceMax)]
    public double Price { get; set; }


    [XmlElement("ProductionDate")]
    [Required]
    public string ProductionDate { get; set; } = null!;


    [XmlElement("ExpiryDate")]
    [Required]
    public string ExpiryDate { get; set; } = null!;


    [XmlElement("Producer")]
    [Required]
    [MaxLength(ValidationConstants.ProducerMAxLENGHT)]
    [MinLength(ValidationConstants.ProsucerMiN)]
    public string Producer { get; set; } = null!;


}
