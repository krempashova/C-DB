
using Cadastre.Data.Enumerations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ImportDtos;

public class ImportCitizenDto
{

    [JsonProperty("FirstName")]
    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string FirstName { get; set; } = null!;

    [JsonProperty("LastName")]
    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string LastName { get; set; } = null!;

    [JsonProperty("BirthDate")]
    [Required]
    public string BirthDate { get; set; } = null!;

    [JsonProperty("MaritalStatus")]
    [Required]
   // [Range(0, 3)]
    public string MaritalStatus { get; set; } = null!;


    [JsonProperty("Properties")]
    public int[] Properties { get; set; } = null!;

}
