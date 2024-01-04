namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Cadastre.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            XmlHelper xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();
            ImportDistrictDto[] districtDtos
                = xmlHelper.Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");
            ICollection<District> validdistricts = new HashSet<District>();
            foreach (ImportDistrictDto dsDto in districtDtos)
            {

                if(!IsValid(dsDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    
                }

                //MAYBE IN VALIDDISTICNKS
                 if(dbContext.Districts.Any(d=>d.Name==dsDto.Name))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District district = new District()
                 { 
                    Region=(Region)dsDto.Region,
                    Name=dsDto.Name,
                    PostalCode=dsDto.PostalCode            
                };

                foreach (ImportPropertyDto proDto in dsDto.Properties)
                {
                    if(!IsValid(proDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime DateOfAcquisition;
                    bool IsDateOfAcquisitionValid = DateTime.TryParseExact(proDto.DateOfAcquisition,"dd/MM/yyyy",CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateOfAcquisition);

                    if(!IsDateOfAcquisitionValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if(dbContext.Properties.Any(p=>p.PropertyIdentifier==proDto.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if(dbContext.Properties.Any(p=>p.Address==proDto.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property property = new Property()
                    {
                        PropertyIdentifier=proDto.PropertyIdentifier,
                        Area=proDto.Area,
                        Details=proDto.Details,
                        Address=proDto.Address,
                        DateOfAcquisition= DateOfAcquisition
                    };
                    district.Properties.Add(property);
                }
                validdistricts.Add(district);
                sb.AppendLine(String.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count));


            }
            dbContext.Districts.AddRange(validdistricts);
            dbContext.SaveChanges();
            return sb.ToString().TrimEnd();


        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();
            ImportCitizenDto[] citizenDtos
                = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);
            ICollection<Citizen> validCitizens = new HashSet<Citizen>();


            foreach (ImportCitizenDto czDto in citizenDtos)
            {
                if(!IsValid(czDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                //if (czDto.MaritalStatus != "Unmarried"
                //    || czDto.MaritalStatus != "Married"
                //    || czDto.MaritalStatus != "Divorced"
                //        || czDto.MaritalStatus != "Widowed")
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                MaritalStatus maritalStatus;
                if(!Enum.TryParse<MaritalStatus>(czDto.MaritalStatus,out maritalStatus))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime birthday;
                bool IsbirthdayValid = DateTime.TryParseExact(czDto.BirthDate,"dd-MM-yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out birthday);
                if(!IsbirthdayValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Citizen citizen = new Citizen()
                {
                    FirstName=czDto.FirstName,
                    LastName=czDto.LastName,
                    BirthDate=birthday,
                    MaritalStatus=maritalStatus
                };

                foreach (int  propId in czDto.Properties)
                {
                    PropertyCitizen propertyCitizen = new PropertyCitizen()
                    { 
                        Citizen=citizen,
                        PropertyId=propId
                    
                    };
                    citizen.PropertiesCitizens.Add(propertyCitizen);

                }

                validCitizens.Add(citizen);
                sb.AppendLine(String.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count));

            }
            dbContext.Citizens.AddRange(validCitizens);
            dbContext.SaveChanges();
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
