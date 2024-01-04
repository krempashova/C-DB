
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Medicament
{
    [Key]
    public int MedicamentId { get; set; }
    [Required]
    [MaxLength(ValidationConstants.MedicamentNameMaxLENGTH)]
    [Unicode(true)]
    public string Name { get; set; } = null!;
}
