using Medicines.Common;
using Medicines.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientsDto
    {

        [JsonProperty("FullName")]
        [Required]
        [MaxLength(ValidationConstants.PatientFullNmaemaxLength)]
        [MinLength(ValidationConstants.NameMin)]
        public string FullName { get; set; }


        [JsonProperty("AgeGroup")]
        [Required]
        [Range(ValidationConstants.AgeMin,ValidationConstants.AgeMAX)]
        public AgeGroup  AgeGroup { get; set; }


        [JsonProperty("Gender")]
        [Required]
        [Range(ValidationConstants.GenderMin,ValidationConstants.genderMax)]
        public Gender  Gender { get; set; }


        [JsonProperty("Medicines")]
        public int[] MedicineIds {get; set; }


    }
}
