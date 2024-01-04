
using Footballers.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto;


public class ImportTeamDto
{

    [Required]
    [MaxLength(40)]
    [MinLength(3)]
    [RegularExpression(Validations.RegexNameTeams)]
    public string Name{ get; set; } = null!;

    [Required]
    [MaxLength(40)]
    [MinLength(2)]
    public string Nationality{ get; set; } = null!;

    [Required]
    public int Trophies { get; set; }

    public int[] Footballers { get; set; } = null!;

}
