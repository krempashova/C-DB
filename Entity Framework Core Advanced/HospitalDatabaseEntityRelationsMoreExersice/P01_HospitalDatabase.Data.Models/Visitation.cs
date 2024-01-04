
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Visitation
{
    [Key]
    public int VisitationId { get; set; }
    public DateTime Date { get; set; }

    [MaxLength(ValidationConstants.VisitationsCommnetsMaxLenght)]
    [Unicode(true)]
    public string? Comments { get; set; }

    [Required]
    public Patient Patient { get; set; } = null!;
}