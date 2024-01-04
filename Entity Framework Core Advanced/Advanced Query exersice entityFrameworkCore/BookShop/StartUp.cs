namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Net.WebSockets;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            string input = Console.ReadLine();

             string  result= GetBooksReleasedBefore(db,input);
           // string result = GetBooksByPrice(db);

            Console.WriteLine(result);




        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {

            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
                string[] bookTitle = context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, bookTitle);
            }

            catch (Exception e)
            {

                return null;
            }
        }

        //GoldenBooks
        public static string GetGoldenBooks(BookShopContext context)
        {
            string  [] goldenBooks = context.Books
                .Where(b => b.EditionType ==EditionType.Gold && b.Copies< 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            return string.Join(Environment.NewLine, goldenBooks);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {

            StringBuilder sb = new StringBuilder();
            
           var  booksByprice = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
               
                .ToArray();

            return string.Join(Environment.NewLine, booksByprice);

          
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            string[] bookreleaseDATE = context.Books
                .Where(b => b.ReleaseDate!.Value.Year != year && b.ReleaseDate.HasValue)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            return String.Join(Environment.NewLine, bookreleaseDATE);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] category = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
            var books = context.Books
               .Where(b => b.BookCategories.Any(bc => category.Contains(bc.Category.Name.ToLower())))
               .OrderBy(b => b.Title)
               .Select(b => b.Title)
               .ToArray();
            return string.Join(Environment.NewLine, books);

        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            try
            {
                DateTime parseDateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string[] books = context.Books
                    .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate < parseDateTime)
                    .OrderByDescending(b => b.ReleaseDate)
                    .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                    .ToArray();
                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var autors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a=>a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();
            return string.Join(Environment.NewLine, autors);

        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {

            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
              
               
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.
                 Count(b => b.Title.Length > lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var autors = context.Authors
              .Select(a => new
              {
                  autorsfullname = $"{a.FirstName} {a.LastName}",
                  bookcopies = a.Books.Sum(b => b.Copies)
              }).OrderByDescending(a => a.bookcopies)
              .Select(a => $"{a.autorsfullname} - {a.bookcopies}")
              .ToArray();
            return string.Join(Environment.NewLine, autors);
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var categoriesWithProfit = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks
                        .Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName);

            foreach (var c in categoriesWithProfit)
            {
                sb.AppendLine($"{c.CategoryName} ${c.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {

            StringBuilder sb = new StringBuilder();
            var category = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryNAME = c.Name,
                    MostreceinTBooks = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Take(3)
                    .Select(cb => new
                    {
                        BookTitle = cb.Book.Title,
                        RealseDate = cb.Book.ReleaseDate.Value.Year
                    }).ToArray()
                }).ToArray();
            foreach (var c in category)
            {
                sb.AppendLine($"--{c.CategoryNAME}");
                foreach (var item in c.MostreceinTBooks)
                {
                    sb.AppendLine($"{item.BookTitle} ({item.RealseDate})");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var privetorelase = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();
            foreach (var pr in privetorelase)
            {
                pr.Price += 5;
               
            }
            context.SaveChanges();
            
        }

        public static int RemoveBooks(BookShopContext context)
        {

            context.ChangeTracker.Clear();

            var books = context.Books
                .Where(b => b.Copies < 4200);

            context.RemoveRange(books);

            return context.SaveChanges();

        }
    }
}


