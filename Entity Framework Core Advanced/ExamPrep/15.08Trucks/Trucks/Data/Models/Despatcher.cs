
using System.ComponentModel.DataAnnotations;
using Trucks.Common;

namespace Trucks.Data.Models;

public class Despatcher
{

    public Despatcher()
    {
        this.Trucks = new HashSet<Truck>();
    }
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(ValidationConstants.DespecherNameMaxLenght)]
    public string Name { get; set; } = null!;
    public string? Position  { get; set; }
    public virtual ICollection<Truck> Trucks { get; set; }
}
