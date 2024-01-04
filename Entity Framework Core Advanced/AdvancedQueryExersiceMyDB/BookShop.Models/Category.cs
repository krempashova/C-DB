
using BookShop.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models;

public class Category
{

    public Category()
    {
        this.BookCategories = new HashSet<BookCategory>();
    }
    [Key]
    public int CategoryId { get; set; }
 

    [Required]
    [Unicode(true)]
    [MaxLength(ValidationConstants.CategorynamwMAXLENGTH)]
    public string Name { get; set; } = null!;
    public virtual ICollection<BookCategory> BookCategories { get; set; }


}
