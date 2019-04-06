using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VaporStore.Data.Enums;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor
{
    using Data;
    using System;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                .Where(g => genreNames.Contains(g.Name) && g.Games.Any(p => p.Purchases.Any()))
                .Select(x => new
                {
                    x.Id,
                    Genre = x.Name,
                    Games = x.Games.Where(g => g.Purchases.Any())
                        .Select(g => new
                        {
                            g.Id,
                            Title = g.Name,
                            Developer = g.Developer.Name,
                            Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
                            Players = g.Purchases.Count
                        })
                        .OrderByDescending(p => p.Players)
                        .ThenBy(p => p.Id),
                    TotalPlayers = x.Games.Sum(g => g.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToList();

            var result = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return result;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var result = new StringBuilder();

            Enum.TryParse<PurchaseType>(storeType, out var purchaseType);

            var purchase = context.Purchases.Where(p => p.Type == purchaseType).Include(c => c.Card).ThenInclude(u => u.User).Include(g => g.Game).ThenInclude(g => g.Genre).ToList();

            var extractedUsers = new Dictionary<string, List<Purchase>>();

            foreach (var line in purchase)
            {
                var username = line.Card.User.Username;

                if (!extractedUsers.ContainsKey(username))
                {
                    extractedUsers.Add(username, new List<Purchase>());
                }

                extractedUsers[username].Add(line);
            }

            var users = new List<ExportUserDto>();

            foreach (var user in extractedUsers)
            {
                var currentDto = new ExportUserDto
                {
                    Username = user.Key,
                    ExportPurchaseDtos = new List<ExportPurchaseDto>()
                };

                foreach (var line in user.Value)
                {
                    var exportPurchase = new ExportPurchaseDto
                    {
                        Card = line.Card.Number,
                        Cvc = line.Card.Cvc,
                        Date = line.Date.ToString("yyyy-MM-dd HH:mm"),
                        ExportGameDto = new ExportGameDto
                        {
                            Title = line.Game.Name,
                            Genre = line.Game.Genre.Name,
                            Price = line.Game.Price
                        }
                    };

                    currentDto.ExportPurchaseDtos.Add(exportPurchase);
                }

                currentDto.TotalSpent = currentDto.ExportPurchaseDtos.Sum(x => x.ExportGameDto.Price);

                currentDto.ExportPurchaseDtos = currentDto.ExportPurchaseDtos.OrderBy(p => p.Date).ToList();
                users.Add(currentDto);
            }

            users = users.OrderByDescending(u => u.TotalSpent).ThenBy(u => u.Username).ToList();
            var serializer = new XmlSerializer(typeof(List<ExportUserDto>), new XmlRootAttribute("Users"));

            serializer.Serialize(new StringWriter(result), users, StartUp.Namespaces);

            return result.ToString().TrimEnd();
        }
    }

    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray("Purchases")]
        public List<ExportPurchaseDto> ExportPurchaseDtos { get; set; }

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }
    }

    [XmlType("Purchase")]
    public class ExportPurchaseDto
    {
        [XmlElement("Card")]
        public string Card { get; set; }

        [XmlElement("Cvc")]
        public string Cvc { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("Game")]
        public ExportGameDto ExportGameDto { get; set; }
    }

    //[XmlType("Game")]
    public class ExportGameDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("Genre")]
        public string Genre { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }

    //<User username="mgraveson">
    //<Purchases>
    //<Purchase>
    //<Card>7991 7779 5123 9211</Card>
    //<Cvc>340</Cvc>
    //<Date>2017-08-31 17:09</Date>
    //<Game title="Counter-Strike: Global Offensive">
    //<Genre>Action</Genre>
    //<Price>12.49</Price>
    //</Game>
    //</Purchase>
    //<Purchase>
    //<Card>7790 7962 4262 5606</Card>
    //<Cvc>966</Cvc>
    //<Date>2018-02-28 08:38</Date>
    //<Game title="Tom Clancy's Ghost Recon Wildlands">
    //<Genre>Action</Genre>
    //<Price>59.99</Price>
    //</Game>
    //</Purchase>
    //</Purchases>
    //<TotalSpent>72.48</TotalSpent>
    //</User>
}