
using Medicines.Common;
using Medicines.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ImportDtos;

public class ImportPatientsDto
{
    [Required]
    [JsonProperty("FullName")]
    [MaxLength(ValidationConstants.PatientFullNmaemaxLength)]
    [MinLength(ValidationConstants.NameMin)]
    public string FullName { get; set; } = null!;


    [JsonProperty("AgeGroup")]
    [Required]
    [Range(ValidationConstants.AgeMin,ValidationConstants.AgeMAX)]
    public int AgeGroup { get; set; }


    [JsonProperty("Gender")]
    [Required]
    [Range(ValidationConstants.GenderMin,ValidationConstants.genderMax)]
    public int Gender { get; set; }

    [JsonProperty("Medicines")]
    [Required]
    public int[] Medicines { get; set; } 

}
