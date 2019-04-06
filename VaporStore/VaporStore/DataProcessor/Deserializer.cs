using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VaporStore.Data.Dto.Import;
using VaporStore.Data.Enums;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor
{
    using Data;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var games = new List<Game>();
            //var gameTags = new List<GameTag>();

            var importedGames = JsonConvert.DeserializeObject<List<ImportGameDto>>(jsonString);
            var developers = context.Developers.ToList();
            var genres = context.Genres.ToList();
            var allTags = context.Tags.ToList();

            foreach (var importGameDto in importedGames)
            {
                if (!IsValid(importGameDto))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var developer = developers.FirstOrDefault(d => d.Name == importGameDto.Developer);

                if (developer == null)
                {
                    developers.Add(new Developer
                    {
                        Name = importGameDto.Developer
                    });
                    developer = developers.FirstOrDefault(d => d.Name == importGameDto.Developer);
                }

                var genre = genres.FirstOrDefault(g => g.Name == importGameDto.Genre);

                if (genre == null)
                {
                    genres.Add(new Genre
                    {
                        Name = importGameDto.Genre
                    });

                    genre = genres.FirstOrDefault(g => g.Name == importGameDto.Genre);
                }

                var game = new Game
                {
                    Developer = developer,
                    Genre = genre,
                    Name = importGameDto.Name,
                    Price = importGameDto.Price,
                    ReleaseDate = importGameDto.ReleaseDate
                };

                foreach (var dtoTag in importGameDto.Tags)
                {
                    var tag = allTags.FirstOrDefault(t => t.Name == dtoTag);

                    if (tag == null)
                    {
                        allTags.Add(new Tag { Name = dtoTag });
                        tag = allTags.FirstOrDefault(t => t.Name == dtoTag);
                    }

                    game.GameTags.Add(new GameTag { Game = game, Tag = tag });
                }

                games.Add(game);
                result.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.AddRange(games);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var users = new List<User>();

            var importedUsers = JsonConvert.DeserializeObject<List<ImportUserDto>>(jsonString);

            foreach (var importUserDto in importedUsers)
            {
                if (!IsValid(importUserDto))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    Username = importUserDto.Username,
                    Age = importUserDto.Age,
                    Email = importUserDto.Email,
                    FullName = importUserDto.FullName,
                    Cards = importUserDto.Cards.Select(x => new Card
                    {
                        Cvc = x.Cvc,
                        Number = x.Number,
                        Type = x.Type
                    }).ToList()
                };

                users.Add(user);
                result.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.AddRange(users);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(List<ImportPurchaseDto>), new XmlRootAttribute("Purchases"));

            var importedPurchases = (List<ImportPurchaseDto>)serializer.Deserialize(new StringReader(xmlString));

            var cards = context.Cards.ToList();

            var games = context.Games.ToList();

            var purchases = new List<Purchase>();

            foreach (var importPurchaseDto in importedPurchases)
            {
                var card = cards.FirstOrDefault(c => c.Number == importPurchaseDto.Card);
                var game = games.FirstOrDefault(g => g.Name == importPurchaseDto.Title);

                if (!IsValid(importPurchaseDto) || card == null || game == null )
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var enumType = Enum.Parse<PurchaseType>(importPurchaseDto.Type);
                var purchase = new Purchase
                {
                    Card = card,
                    Date = DateTime
                        .ParseExact(importPurchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Game = game,
                    ProductKey = importPurchaseDto.ProductKey,
                    Type = enumType
                };

                purchases.Add(purchase);
                result.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);

            var validationResult = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}