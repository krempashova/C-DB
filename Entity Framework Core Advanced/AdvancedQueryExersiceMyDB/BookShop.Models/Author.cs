using Microsoft.EntityFrameworkCore;
using BookShop.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models;

public class Author
{

    public Author()
    {
        this.Books = new HashSet<Book>();
    }

    [Key]
    public int AuthorId { get; set; }

    [MaxLength(ValidationConstants.AuthorFirstNAMEMAXLENGT)]
    [Unicode(true)]
      public string? FirstName{ get; set; }


    [MaxLength(ValidationConstants.AutorLastNameMaxLength)]
    [Unicode(true)]
    public string?LastName { get; set; }

    public virtual ICollection<Book> Books { get; set; }
}