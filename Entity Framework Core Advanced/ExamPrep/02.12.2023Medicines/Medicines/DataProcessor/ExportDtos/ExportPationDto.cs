
using Medicines.Data.Models.Enums;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos;
[XmlType("Patient")]
public class ExportPationDto
{
    [XmlElement("Name")]
    public string FullName { get; set; } = null!;

    [XmlElement("AgeGroup")]
    public string AgeGroup { get; set; } = null!;

    [XmlAttribute("Gender")]
    public string Gender { get; set; } = null!;

    [XmlArray("Medicines")]
    public ExportMedicinesDto[] Medicines { get; set; } = new ExportMedicinesDto[0];
}
