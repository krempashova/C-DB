namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";
        private static XmlHelper xmlHelper;
       
        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
           
            StringBuilder sb = new StringBuilder();
            xmlHelper = new XmlHelper();
            ImportDespacherDto[] despacherDtos
                = xmlHelper.Deserialize<ImportDespacherDto[]>(xmlString, "Despatchers");

            ICollection<Despatcher> validdespatchers = new HashSet<Despatcher>();
            foreach (ImportDespacherDto despacherDto in despacherDtos)
            {
               
                if (!IsValid(despacherDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Truck> validTrucks = new HashSet<Truck>();
                foreach (ImportTruckDto truckDto in despacherDto.Truks)
                {

                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Truck truck = new Truck()
                    {
                        
                    
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType
                    };

               

                    validTrucks.Add(truck);
                }
                Despatcher despatcher = new Despatcher()
                { 
                    
                    Name=despacherDto.Name,
                    Position=despacherDto.Position,
                    Trucks=validTrucks


                };

                validdespatchers.Add(despatcher);

                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher.Name, validTrucks.Count()));
            }

            context.Despatchers.AddRange(validdespatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportClientDto[] clientDtos
                = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);
            ICollection<Client> validclients = new HashSet<Client>();

            ICollection<int>existigTrucksID= context.Trucks
                .Select(t => t.Id)
                .ToArray();

            foreach (ImportClientDto clientDto in clientDtos)
            {
                if(!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(clientDto.Type=="usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client client = new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type
                };

                foreach (int truckId in clientDto.TruckIds.Distinct())
                {
                    if (!existigTrucksID.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck ct = new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId
                    };
                    client.ClientsTrucks.Add(ct);
                }

                validclients.Add(client);
                sb
                    .AppendLine(String.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }

            context.Clients.AddRange(validclients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static IMapper InitializeAutoMapper()
          => new Mapper(new MapperConfiguration(cfg =>
          {
              cfg.AddProfile<TrucksProfile>();
          }));

    }
}