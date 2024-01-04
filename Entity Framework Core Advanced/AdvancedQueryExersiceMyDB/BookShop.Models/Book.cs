using BookShop.Data.Common;
using BookShop.Models.BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models;

public class Book
{
    [Key]
    public int BookId { get; set; }

    [MaxLength(ValidationConstants.BookTitleMaxlength)]
    [Unicode(true)]
    public string? Title { get; set; }

    [Unicode(true)]
    [MaxLength(ValidationConstants.BookDescriptionMaxLength)]
    public string? Description { get; set; }

    public DateTime? ReleaseDate { get; set; }
    public int Copies { get; set; }
    public decimal Price { get; set; }
    public EditionType EditionType { get; set; }
    public AgeRestriction AgeRestriction { get; set; }

    [ForeignKey(nameof(Author))]
    public int  AuthorId { get; set; }
    public virtual Author Autor { get; set; } = null!;
    

}
