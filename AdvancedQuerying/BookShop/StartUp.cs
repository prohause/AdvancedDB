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
                IncreasePrices(db);
            }
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(b => ((DateTime)b.ReleaseDate).Year < 2010).ToList();
            books.ForEach(b => b.Price += 5);
            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var category = context.Categories.Select(c => new
            {
                c.Name,
                Books = c.CategoryBooks.OrderByDescending(cb => cb.Book.ReleaseDate).Take(3)
            }).OrderBy(c => c.Name).Select(x => $"--{x.Name}" + Environment.NewLine + string.Join(Environment.NewLine, x.Books.Select(b => $"{b.Book.Title} ({((DateTime)b.Book.ReleaseDate).Year})")))
                .ToList();

            return string.Join(Environment.NewLine, category);
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profitByCategory = context.Categories.Select(c => new
            {
                c.Name,
                Profit = c.CategoryBooks.Select(cb => cb.Book.Copies * cb.Book.Price).Sum()
            }).ToList().OrderByDescending(x => x.Profit).Select(x => $"{x.Name} ${x.Profit:F2}").ToList();

            return string.Join(Environment.NewLine, profitByCategory);
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