﻿
using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto;


[XmlType("Invoice")]
public class ExportInvoiceDto
{

    [Required]
    [XmlElement("InvoiceNumber")]
    public int InvoiceNumber { get; set; }
  
    [XmlElement("InvoiceAmount")]
    public decimal InvoiceAmount { get; set; }

    [Required]
    [XmlElement("DueDate")]
    public string DueDate { get; set; }

   

    [XmlElement("Currency")]
    public string Currency { get; set; }

}
