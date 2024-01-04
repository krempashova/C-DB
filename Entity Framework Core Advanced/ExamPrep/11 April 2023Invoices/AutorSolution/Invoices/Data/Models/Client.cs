using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Invoices.Shared.GlobalConstants;

namespace Invoices.Data.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public ICollection<ProductClient> ProductsClients { get; set; } = new List<ProductClient>();

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
