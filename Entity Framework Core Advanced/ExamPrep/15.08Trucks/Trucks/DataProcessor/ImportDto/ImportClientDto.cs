
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Trucks.Common;

namespace Trucks.DataProcessor.ImportDto;

public class ImportClientDto
{
    [JsonProperty("Name")]
    [Required]
    [MinLength(ValidationConstants.CLIENTNamemin)]
    [MaxLength(ValidationConstants.DespecherNameMaxLenght)]
    public string Name { get; set; } = null!;

    [JsonProperty("Nationality")]
    [Required]
    [MinLength(ValidationConstants.clientnationalityMIN)]
    [MaxLength(ValidationConstants.ClientNationalityMaxlength)]
    public string Nationality { get; set; } = null!;

    [JsonProperty("Type")]
    [Required]
    public string Type { get; set; } = null!;

    [JsonProperty("Trucks")]
    public int [] TruckIds{ get; set; }

}
