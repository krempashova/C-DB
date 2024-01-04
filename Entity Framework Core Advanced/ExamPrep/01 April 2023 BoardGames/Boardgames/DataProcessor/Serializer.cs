namespace Boardgames.DataProcessor
{

    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Castle.DynamicProxy.Generators;
    using Microsoft.Data.SqlClient.Server;
    using Newtonsoft.Json;
    using System.Text;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {


            XmlHelper xmlHelper = new XmlHelper();

            var creators = context.Creators
                .Where(c => c.Boardgames.Count > 0)
                .Select(c => new ExportCreatorDto()
                {
                    BoardgamesCount = c.Boardgames.Count(),
                    CreatorName = c.FirstName + " " + c.LastName,
                    Boardgames = c.Boardgames
                    .Select(bg => new ExportBoardGamesDto()
                    {
                        BoardgameName = bg.Name,
                        BoardgameYearPublished = bg.YearPublished
                    })
                    .OrderBy(bg => bg.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();



            return xmlHelper.Serialize(creators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                 .Where(s => s.BoardgamesSellers.Any(bg=>bg.Boardgame.YearPublished >= year
                 && bg.Boardgame.Rating <= rating))
                 .Select(s => new
                 {
                     Name = s.Name,
                     Website = s.Website,
                     Boardgames = s.BoardgamesSellers
                     .Where(bg => bg.Boardgame.YearPublished >= year && bg.Boardgame.Rating <= rating)
                     .Select(bg => new
                     {
                         Name = bg.Boardgame.Name,
                         Rating = bg.Boardgame.Rating,
                         Mechanics = bg.Boardgame.Mechanics,
                         Category = bg.Boardgame.CategoryType.ToString(),
                     }).OrderByDescending(bg => bg.Rating)
                     .ThenBy(bg => bg.Name)
                     .ToArray()
                 })
                 .OrderByDescending(s => s.Boardgames.Count())
                 .ThenBy(s => s.Name)
                 .Take(5)
                 .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}