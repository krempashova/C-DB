namespace Medicines.DataProcessor
{
    using Medicines.Utilities;
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Linq;
    using System.Globalization;
    using System.Diagnostics;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportPatientsDto[] patientsDtos
                = JsonConvert.DeserializeObject<ImportPatientsDto[]>(jsonString);

            ICollection<Patient> validpatients = new HashSet<Patient>();
            foreach (ImportPatientsDto pDto in patientsDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient()
                {

                    FullName = pDto.FullName,
                    AgeGroup = (AgeGroup)pDto.AgeGroup,
                    Gender = (Gender)pDto.Gender,
                };

                foreach (int medID in pDto.Medicines)
                {
                    if (patient.PatientsMedicines.Any(x => x.MedicineId == medID))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine patientMedicine = new PatientMedicine()
                    {
                        Patient = patient,
                        MedicineId = medID,
                    };

                    patient.PatientsMedicines.Add(patientMedicine);
                }

                validpatients.Add(patient);
                sb.AppendLine(String.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));



            }


            context.Patients.AddRange(validpatients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            ImportPharmacyDto[] pharmacyDtos = xmlHelper.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");
            ICollection<Pharmacy> validPharmacy = new HashSet<Pharmacy>();
            foreach (ImportPharmacyDto PhDto in pharmacyDtos)
            {

                if(!IsValid(PhDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Pharmacy pharmacy = new Pharmacy()
                {

                    IsNonStop = bool.Parse(PhDto.IsNonStop),
                    Name = PhDto.Name,
                    PhoneNumber = PhDto.PhoneNumber,

                };

                foreach (ImportMedicinesDto medDto in PhDto.Medicines)
                {
                    if (!IsValid(medDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if(pharmacy.Medicines.Any(x=>x.Name==medDto.Name)&&
                        pharmacy.Medicines.Any(x=>x.Producer==medDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime medicineProductionDate;
                    bool isProductionDateValid = DateTime
                        .TryParseExact(medDto.ProductionDate, "yyyy-MM-dd", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out medicineProductionDate);
                    if (!isProductionDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime medicineExpityDate;
                    bool isExpityDate = DateTime
                        .TryParseExact(medDto.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out medicineExpityDate);

                    if(!isExpityDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if(medicineProductionDate>=medicineExpityDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new Medicine()
                    {
                        Name = medDto.Name,
                        Price = (decimal)medDto.Price,
                        Category = (Category)medDto.Category,
                        ProductionDate = medicineProductionDate,
                        ExpiryDate = medicineExpityDate,
                        Producer = medDto.Producer,
                    };
                    pharmacy.Medicines.Add(medicine);
                }

                validPharmacy.Add(pharmacy);
                sb.AppendLine(string
                   .Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));

            }

            context.Pharmacies.AddRange(validPharmacy);
            context.SaveChanges();
            return sb.ToString().TrimEnd();




        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
