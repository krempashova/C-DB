
using Medicines.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos;


[XmlType("Pharmacy")]
public class ImportPharmacyDto
{
    [XmlAttribute("non-stop")]
    [Required]
    public bool IsNonStop { get; set; } 


    [XmlElement("Name")]
    [MaxLength(ValidationConstants.PharmacyNameMaxLenght)]
    [MinLength(ValidationConstants.pharmacyMin)]
    public string Name { get; set; } = null!;


    [XmlElement("PhoneNumber")]
    [Required]
    [MaxLength(ValidationConstants.PharmacyPhoneNuMBERLenght)]
    [MinLength(ValidationConstants.PharmacyPhoneNuMBERLenght)]
    [RegularExpression(ValidationConstants.RegexPhoneNumber)]
    public string PhoneNumber { get; set; } = null!;


    [XmlArray("Medicines")]
    public ImportMedicineDto[] Medicines { get; set; } = null!;

}
