
using Invoices.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Invoices.Data.Models;

public class Address
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(Validations.AdresStreetNameMaxlength)]
    public string StreetName { get; set; } = null!;

    [Required]
    public int StreetNumber { get; set; }


    [Required]
    public string PostCode { get; set; } = null!;

    [Required]
    [MaxLength(Validations.CityMaxLengt)]
    public string City { get; set; } = null!;

    [Required]
    [MaxLength(Validations.CityMaxLengt)]
    public string Country { get; set; } = null!;

    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public virtual Client Client { get; set; } = null!;
}
