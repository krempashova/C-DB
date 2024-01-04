
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models;

public class Property
{

    public Property()
    {
        this.PropertiesCitizens = new HashSet<PropertyCitizen>();
    }
    [Key]
    public int Id { get; set; }


    [Required]
    [MaxLength(20)]
    public string PropertyIdentifier { get; set; } = null!;

    [Required]
    public int Area  { get; set; }


    [MaxLength(500)]
    public string? Details { get; set; }


    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = null!;


    [Required]
    public DateTime DateOfAcquisition  { get; set; }


    [ForeignKey(nameof(District))]
    public int DistrictId  { get; set; }

    public virtual District District { get; set; } = null!;

    public  virtual ICollection<PropertyCitizen> PropertiesCitizens  { get; set; }


}
