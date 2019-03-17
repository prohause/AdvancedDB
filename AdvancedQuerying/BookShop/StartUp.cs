using BookShop.Models.Enums;
using System;
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
                var result = GetBooksByCategory(db, "horror mystery drama");
                Console.WriteLine(result);
            }
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
    }
}