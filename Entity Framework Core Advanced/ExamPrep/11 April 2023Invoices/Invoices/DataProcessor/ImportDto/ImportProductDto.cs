﻿
using Invoices.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto;

public class ImportProductDto
{
    [Required]
    [MaxLength(30)]
    [MinLength(9)]
    public string Name { get; set; } = null!;


    [Required]
    [Range(5.00,1000.00)]
    public decimal Price { get; set; }


    [Required]
    [Range(0,4)]
    public CategoryType CategoryType { get; set; }


    [JsonProperty("Clients")]
    public int[] ClientIds { get; set; }
}
