
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models.Enums;
using StydentSystem.Data.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Resource
{
    [Key]
    public int ResourceId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ResoursecNameMaxlength)]
    [Unicode(true)]
    public string Name { get; set; } = null!;

    [Unicode(false)]
    [MaxLength(ValidationConstants.ResoursecURLMaxlength)]
    public string? Url { get; set; }

    public ResourceType ResourceType  { get; set; }

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
}