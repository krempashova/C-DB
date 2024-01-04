
using Medicines.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos;


[XmlType("Pharmacy")]
public class ImportPharmacyDto
{

    [XmlAttribute("non-stop")]
    [Required]
    [RegularExpression(ValidationConstants.PharmacyBooleanRegex)]
    public string IsNonStop { get; set; }


    [XmlElement("Name")]
    [Required]
    [MaxLength(ValidationConstants.PharmacyNameMaxLenght)]
    [MinLength(ValidationConstants.pharmacyMin)]
    public string Name { get; set; }


    [XmlElement("PhoneNumber")]
    [RegularExpression(ValidationConstants.RegexPhoneNumber)]
    [MinLength(ValidationConstants.PharmacyPhoneNuMBERLenght)]
    [Required]
    [MaxLength(ValidationConstants.PharmacyPhoneNuMBERLenght)]
    public string PhoneNumber { get; set; }


     [XmlArray("Medicines")]
    public ImportMedicinesDto[] Medicines { get; set; }
}
