

using Medicines.Common;
using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models;

public class Medicine
{

    public Medicine()
    {
        this.PatientsMedicines = new HashSet<PatientMedicine>();
    }
    [Key]
    public  int Id { get; set; }


    [Required]
    [MaxLength(ValidationConstants.MedicinenameMaxlenght)]
    public string Name { get; set; } = null!;

    [Required]
    public decimal Price  { get; set; }

    [Required]
    public Category Category { get; set; }


    [Required]
    public DateTime ProductionDate  { get; set; }

    [Required]
    public DateTime ExpiryDate  { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ProducerMAxLENGHT)]
    public string Producer { get; set; } = null!;


    [Required]
    [ForeignKey(nameof(Pharmacy))]
    public int PharmacyId  { get; set; }
    public virtual Pharmacy Pharmacy { get; set; } = null!;

    public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; } = null!;
}
