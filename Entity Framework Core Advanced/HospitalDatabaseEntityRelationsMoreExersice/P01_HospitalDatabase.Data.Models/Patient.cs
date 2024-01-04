using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace P01_HospitalDatabase.Data.Models;

    public class Patient
    {
    [Key]
    public int PatientId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.PatientFirstnameMaxLenght)]
    [Unicode(true)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.PatientLastNAMEMaxLength)]
    [Unicode(true)]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.PatientAddressMaxlenght)]
    [Unicode(true)]
    public string Address { get; set; } = null!;

    [MaxLength(ValidationConstants.PatienEmailMaxLenght)]
    [Unicode(false)]
    public string ?Email { get; set; }

    public bool HasInsurance { get; set; }
}
