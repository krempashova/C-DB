
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Diagnose
{
    [Key]
    public int DiagnoseId { get; set; }
    [Required]
    [MaxLength(ValidationConstants.DiagnoseNameMaxLength)]
    [Unicode(true)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.DiagnoseCommentMaxlenght)]
    [Unicode(true) ]    
    public string? Comments  { get; set; }
    public Patient Patient { get; set; } = null!;
}
