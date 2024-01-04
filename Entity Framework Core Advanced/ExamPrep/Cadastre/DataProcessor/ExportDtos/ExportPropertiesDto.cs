
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos;


[XmlType("Property")]
public class ExportPropertiesDto
{

    [XmlAttribute("postal-code")]
    public string PostalCode { get; set; } = null!;

    [XmlElement("PropertyIdentifier")]
   
    public string PropertyIdentifier { get; set; } = null!;


    [XmlElement("Area")]
   
    public int Area { get; set; }


    [XmlElement("DateOfAcquisition")]

    public string DateOfAcquisition { get; set; } 
}
