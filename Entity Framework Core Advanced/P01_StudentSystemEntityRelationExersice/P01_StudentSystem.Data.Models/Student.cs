using Microsoft.EntityFrameworkCore;
using StydentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models;

public class Student
{

    public Student()
    {
        this.Homeworks=new HashSet<Homework>();
        this.StudentsCourses = new HashSet<StudentCourse>();
    }
    [Key]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.StudentNameMaxLength)]
    [Unicode(true)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.SrtudentPhoneNumberMaxlength)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    public DateTime RegisteredOn { get; set; }

    public DateTime? Birthday  { get; set; }

    public virtual ICollection<StudentCourse>? StudentsCourses { get; set; }
    public virtual ICollection<Homework>? Homeworks { get; set; } 
}