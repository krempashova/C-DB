
using Invoices.Common;
using Invoices.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models;

public class Product
{


    public Product()
    {
        this.ProductsClients = new HashSet<ProductClient>();
    }
    [Key]
    public int Id { get; set; }


    [Required]
    [MaxLength(Validations.NameMaxLength)]
    public string Name { get; set; } = null!;


    [Required]
    public decimal Price { get; set; }


    [Required]
    public  CategoryType CategoryType { get; set; }

    public virtual ICollection<ProductClient> ProductsClients { get; set; } = null!;
}
