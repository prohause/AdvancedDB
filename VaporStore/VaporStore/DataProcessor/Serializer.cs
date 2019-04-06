using Newtonsoft.Json;
using System.Linq;

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
                            Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag)),
                            Players = g.Purchases.Count
                        }),
                    TotalPlayers = x.Games.Sum(g => g.Purchases.Count)
                })
                .ToList();

            var result = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return result;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            throw new NotImplementedException();
        }
    }
}