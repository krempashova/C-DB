namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using Medicines.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportPatientsDto[] patientsDtos =
               JsonConvert.DeserializeObject<ImportPatientsDto[]>(jsonString);

            ICollection<Patient> validpatient = new HashSet<Patient>();
            ICollection<int> existingMedidneIDS = context.Medicines
                 .Select(t => t.Id)
                .ToArray();

            foreach (ImportPatientsDto patientsDto in patientsDtos)
            {
                if (!IsValid(patientsDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Patient patient = new Patient()
                {
                    FullName = patientsDto.FullName,
                    AgeGroup = (AgeGroup)patientsDto.AgeGroup,
                    Gender = (Gender)patientsDto.Gender,
                };
                foreach (int medicineId in patientsDto.MedicineIds.Distinct())
                {
                    if (!existingMedidneIDS.Contains(medicineId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine pm = new PatientMedicine()
                    {
                        Patient = patient,
                        MedicineId = medicineId
                    };
                    patient.PatientsMedicines.Add(pm);
                }
                validpatient.Add(patient);
                sb.AppendLine(String.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
            }

            context.Patients.AddRange(validpatient);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


       
        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {

             private static XmlHelper XmlHelper;

        XmlHelper xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();
            ImportPharmacyDto[] pharmacyDtos =
              xmlHelper.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            ICollection<Pharmacy> validPharmacies = new HashSet<Pharmacy>();
            foreach (ImportPharmacyDto pharmacyDto in pharmacyDtos)
            {
                if (!IsValid(pharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!pharmacyDto.IsNonStop == true && !pharmacyDto.IsNonStop == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                ICollection<Medicine> validMedicinies = new HashSet<Medicine>();
                foreach (ImportMedicineDto medicineDto in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicineDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrEmpty(medicineDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (medicineDto.ProductionDate.Day >= medicineDto.ExpiryDate.Day)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if ((validMedicinies.Any(m => m.Name == medicineDto.Name) ||
                        validMedicinies.Any(p => p.Producer == medicineDto.Producer)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    Medicine medicine = new Medicine()
                    {
                        Category = (Category)medicineDto.Category,
                        Name = medicineDto.Name,
                        Price = medicineDto.Price,
                        ProductionDate = medicineDto.ProductionDate,
                        ExpiryDate = medicineDto.ExpiryDate,
                        Producer = medicineDto.Producer
                    };
             
                    validMedicinies.Add(medicine);

                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    IsNonStop = pharmacyDto.IsNonStop,
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                    Medicines = validMedicinies

                };
                validPharmacies.Add(pharmacy);
                sb.AppendLine(String.Format(SuccessfullyImportedPharmacy, pharmacy.Name, validMedicinies.Count));
            }

            context.Pharmacies.AddRange(validPharmacies);
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
