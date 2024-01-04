namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xml = new XmlHelper();

            ImportCreatorDto[] creatorDtos = xml.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            ICollection<Creator> validCreators = new HashSet<Creator>();
            foreach (ImportCreatorDto crDto in creatorDtos)
            {

                if(!IsValid(crDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Creator creator = new Creator()
                {
                    FirstName = crDto.FirstName,
                    LastName = crDto.LastName,
                };

                foreach (var bgDto in crDto.Boardgames)
                {
                    if(string.IsNullOrEmpty(bgDto.Name)) 
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    
                    }


                    if(!IsValid(bgDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    creator.Boardgames.Add(new Boardgame()
                    {
                        Name = bgDto.Name,
                        Rating = bgDto.Rating,
                        YearPublished = bgDto.YearPublished,
                        CategoryType = (CategoryType)bgDto.CategoryType,
                        Mechanics = bgDto.Mechanics
                    });
                }

                validCreators.Add(creator);
                sb.AppendLine(String.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));



            }

            context.Creators.AddRange(validCreators);
            context.SaveChanges();
            return sb.ToString().TrimEnd();





        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {

            StringBuilder sb = new StringBuilder();
            ImportSellerDto[] sellerDtos =
                JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);


            var UniqieBoardGames = context.Boardgames
               .Select(bg => bg.Id)
               .ToArray();

            ICollection<Seller> validSellers = new HashSet<Seller>();

            foreach (ImportSellerDto sellerDto in sellerDtos)
            {
                
               if(!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website

                };
                foreach (var  bgint in sellerDto.Boardgames.Distinct())
                {
                    if(!UniqieBoardGames.Contains(bgint))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    { 
                       Seller=seller,
                       BoardgameId=bgint,                  
                    };

                    seller.BoardgamesSellers.Add(boardgameSeller);

                }
                validSellers.Add(seller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
                 

            };

            context.Sellers.AddRange(validSellers);
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
