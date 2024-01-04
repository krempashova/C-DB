namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albuminfo = context.Albums
                .Where(a => a.ProducerId.HasValue &&
                  a.ProducerId.Value == producerId)
                .ToArray()
                .OrderByDescending(a=>a.Price)
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                     .Select(s => new
                     {
                         SongName = s.Name,
                         PriceSong = s.Price.ToString("F2"),
                         SongWriterName = s.Writer.Name

                     }).ToArray()
                     .OrderByDescending(s => s.SongName)
                     .ThenBy(s => s.SongWriterName)
                     .ToArray(),
                    AlbumTotalPrice = a.Price
                     .ToString("F2")
                }).ToArray();

            foreach (var a in albuminfo)
            {
                sb.AppendLine($"-AlbumName: {a.Name}");
                sb.AppendLine($"-ReleaseDate: {a.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {a.ProducerName}");
                sb.AppendLine($"-Songs:");

                  int songnumber = 1;
                foreach (var s in a.Songs)
                {
                    sb.AppendLine($"---#{songnumber}");
                    sb.AppendLine($"---SongName: {s.SongName}");
                    sb.AppendLine($"---Price: {s.PriceSong}");
                    sb.AppendLine($"---Writer: {s.SongWriterName}");
                    songnumber++;
                }
                sb.AppendLine($"-AlbumPrice: {a.AlbumTotalPrice}");
            }

            return sb.ToString().TrimEnd();


        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();
            var Songsinfo = context.Songs
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                    .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                    .OrderBy(p => p)
                    .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album!.Producer!.Name,
                    Duration = s.Duration.ToString("c"),
                }).OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToArray();
            int songcOUNT = 1;
            foreach (var S in Songsinfo)
            {
                sb.AppendLine($"-Song #{songcOUNT}");
                sb.AppendLine($"---SongName: {S.Name}");
                sb.AppendLine($"---Writer: {S.WriterName}");
                foreach (var p in S.Performers)
                {
                    sb.AppendLine($"---Performer: {p}");
                }
                sb.AppendLine($"---AlbumProducer: {S.AlbumProducer}");
                sb.AppendLine($"---Duration: {S.Duration}");
                songcOUNT++;
            }
            return sb.ToString().TrimEnd();
        }
    }
}
