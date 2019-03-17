using BookShop.Models.Enums;
using System;
using System.Globalization;
using System.Linq;

namespace BookShop
{
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);
                var result = CountCopiesByAuthor(db);
                Console.WriteLine(result);
            }
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var books = context.Authors
                .Select(a => new { FullName = a.FirstName + " " + a.LastName, TotalCopies = a.Books.Select(b => b.Copies).Sum() })
                .OrderByDescending(x => x.TotalCopies).Select(x => $"{x.FullName} - {x.TotalCopies}")
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Count(b => b.Title.Length > lengthCheck);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command.ToLower(), true);
            var books = context.Books.Where(b => b.AgeRestriction == ageRestriction).Select(b => b.Title).OrderBy(x => x).ToList();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000).Select(b => b.Title).ToList();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Price > 40).OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}").ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books.Where(b => ((DateTime)b.ReleaseDate).Year != year).OrderBy(x => x.BookId)
                .Select(b => b.Title).ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToList();

            var books = context.Books
                .Where(b => b.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(b => b.Title).OrderBy(x => x).ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var endDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books.Where(b => b.ReleaseDate < endDate).OrderByDescending(x => x.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}").ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors.Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}").OrderBy(x => x).ToList();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books.Where(b => b.Title.ToLower().Contains(input.ToLower())).Select(b => b.Title)
                .OrderBy(x => x).ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books.Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower())).OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})").ToList();

            return string.Join(Environment.NewLine, books);
        }
    }
}