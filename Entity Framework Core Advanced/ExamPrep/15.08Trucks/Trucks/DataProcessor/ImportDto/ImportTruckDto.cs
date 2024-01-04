
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto;


[XmlType("Truck")]
public class ImportTruckDto
{

    [XmlElement("RegistrationNumber")]
    [MaxLength(ValidationConstants.TruckRegistrationNumerLength)]
    [MinLength(ValidationConstants.TruckRegistrationNumerLength)]
    [RegularExpression(ValidationConstants.TruckRegistrationNumberregex)]
    public string? RegistrationNumber { get; set; }

    [XmlElement("VinNumber")]
    [Required]
    [MaxLength(ValidationConstants.TruckVinNumberlenght)]
    [MinLength(ValidationConstants.TruckVinNumberlenght)]
    public string VinNumber { get; set; } = null!;

    [XmlElement("TankCapacity")]
    [Required]
    [Range(ValidationConstants.TankCapacityMinLength,ValidationConstants.TankCapacityMaxlenght)]
    public int TankCapacity { get; set; }

    [XmlElement("CargoCapacity")]
    [Required]
    [Range(ValidationConstants.CargoCapacityMinlength,ValidationConstants.CargoCapacityMaxLength)]
    public int CargoCapacity { get; set; }

    [XmlElement("CategoryType")]
   
    [Range(ValidationConstants.categoryTypeminlenght,ValidationConstants.categoryTypeMAXLENGTH)]
    public int CategoryType { get; set; }

    [XmlElement("MakeType")]
    
    [Range(ValidationConstants.MakeTypemin,ValidationConstants.MakeTypemax)]
    public int MakeType { get; set; }
}
