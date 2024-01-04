﻿
using Boardgames.Common;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto;

public class ImportSellerDto
{


    [Required]
    [MaxLength(20)]
    [MinLength(5)]
    public string Name { get; set; } = null!;


    [Required]
    [MaxLength(30)]
    [MinLength(2)]
    public string Address { get; set; } = null!;


    [Required]
    public string Country { get; set; } = null!;


    [Required]
    [RegularExpression(Validations.websiteRegexValidation)]

    public string Website { get; set; } = null!;


    [Required]
    public int[] Boardgames { get; set; } = null!;
}
