
using Invoices.Common;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models;

public class Client
{

    public Client()
    {
        this.Invoices = new HashSet<Invoice>();
        this.Addresses = new HashSet<Address>();
        this.ProductsClients = new HashSet<ProductClient>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(Validations.ClientNameMaxLengt)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(Validations.ClientnumberVatmaxlenght)]
    public string NumberVat { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices{ get; set; }

    public  virtual ICollection<Address> Addresses { get; set; }

    public   virtual ICollection<ProductClient> ProductsClients  { get; set; }

}
