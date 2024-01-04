
using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.Data.Models;

public class District
{

    public District()
    {
        this.Properties = new HashSet<Property>();
    }
    [Key]
    public int Id { get; set; }


    [Required]
    [MaxLength(80)]
    public string Name { get; set; } = null!;


    [Required]
    [MaxLength(8)]
    public string PostalCode { get; set; } = null!;

    [Required]
    public Region Region { get; set; }

    public virtual ICollection<Property> Properties  { get; set; }
}
