namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {



            DateTime givendate;
            bool isGivenDateVALID = DateTime
                       .TryParseExact(date, "yyyy-MM-dd", CultureInfo
                       .InvariantCulture, DateTimeStyles.None, out givendate);
            StringBuilder sb = new StringBuilder();
             
            XmlHelper xmlHelper = new XmlHelper();
            ExportPationDto[] patients = context.Patients.AsNoTracking()
                 .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate >= givendate))
                 .Select(p => new ExportPationDto
                 {
                     FullName = p.FullName,
                     AgeGroup = p.AgeGroup.ToString(),
                     Gender = p.Gender.ToString().ToLower(),
                     Medicines = p.PatientsMedicines
                     .Where(pm => pm.Medicine.ProductionDate >= givendate)
                     .Select(pm => pm.Medicine)
                     .OrderByDescending(m => m.ExpiryDate)
                     .ThenBy(m => m.Price)
                     .Select(m => new ExportMedicinesDto
                     {
                         Name = m.Name,
                         Price = m.Price.ToString("F2"),
                         Category = m.Category.ToString().ToLower(),
                         Producer = m.Producer,
                         ExpiryDate = m.ExpiryDate.ToString("yyyy-MM-dd")
                     })
                     .ToArray()
                 })
                 .OrderByDescending(p => p.Medicines.Length)
                 .ThenBy(p => p.FullName)
                 .ToArray();

            return xmlHelper.Serialize( patients  , "Patients");

            

            
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicaments = context.Medicines.AsNoTracking()
                .Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop)
                .ToArray()
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new
                {
                    m.Name,
                    Price = m.Price.ToString("F2"),
                    Pharmacy = new
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber

                    }

                }).ToArray();

            return JsonConvert.SerializeObject(medicaments, Formatting.Indented);
        }
    }
}
