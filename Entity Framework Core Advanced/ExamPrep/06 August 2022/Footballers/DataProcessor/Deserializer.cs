namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {

            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            ImportCoachDto[] coachDtos
                = xmlHelper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");


            ICollection<Coach> validcoaches = new HashSet<Coach>();

            foreach (ImportCoachDto chDto in coachDtos)
            {

                if (!IsValid(chDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (String.IsNullOrEmpty(chDto.Nationality))
                {

                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = chDto.Name,
                    Nationality = chDto.Nationality

                };
                foreach (ImportFootballerDto footballerDto in chDto.Footballers)
                {
                    if(!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime footballerContractStartdate;
                    bool IsfootballerContractStartdateValid = DateTime.TryParseExact(footballerDto.ContractStartDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out footballerContractStartdate);
                    if(!IsfootballerContractStartdateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime footballerContractEnddate;
                    bool IsfootballerContractEnddateValid = DateTime.TryParseExact(footballerDto.ContractEndDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out footballerContractEnddate);
                     if(!IsfootballerContractEnddateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                     if(footballerContractStartdate>=footballerContractEnddate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = footballerContractStartdate,
                        ContractEndDate = footballerContractEnddate,
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType=(PositionType)footballerDto.PositionType
                    };

                    coach.Footballers.Add(footballer);
                }

                validcoaches.Add(coach);

                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));

            }
            context.Coaches.AddRange(validcoaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<int> existingFootballers
                 = context.Footballers
                 .Select(f => f.Id)
                 .ToArray();

            ImportTeamDto[] teamDtos =
                JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            ICollection<Team> validteams = new HashSet<Team>();

            foreach (ImportTeamDto tDto in teamDtos)
            {

                if(!IsValid(tDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(String.IsNullOrEmpty(tDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(tDto.Trophies<=0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Team team = new Team()
                {
                    Name=tDto.Name,
                    Nationality=tDto.Nationality,
                    Trophies=tDto.Trophies, 
                };
                foreach (int footballerId in tDto.Footballers.Distinct())
                {
                    if(!IsValid(footballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if(!existingFootballers.Contains(footballerId))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer tf = new TeamFootballer()
                    {
                      Team=team,
                      FootballerId= footballerId

                    };
                    team.TeamsFootballers.Add(tf);

                }
                validteams.Add(team);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));

            }

            context.Teams.AddRange(validteams);
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
