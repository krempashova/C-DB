
using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.Data.Models;

public class Citizen
{

    public Citizen()
    {
        this.PropertiesCitizens = new HashSet<PropertyCitizen>();
    }
    [Key]
    public int Id { get; set; }


    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; } = null!;


    [Required]
    [MaxLength(30)]
    public string LastName { get; set; } = null!;


    [Required]
    public DateTime BirthDate  { get; set; }

    [Required]
    public  MaritalStatus MaritalStatus { get; set; }

    public  virtual ICollection<PropertyCitizen> PropertiesCitizens  { get; set; }
}
