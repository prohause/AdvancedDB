using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                var input = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\ProductShop\ProductShop\Datasets\products.json");

                var result = ImportProducts(context, input);
                Console.WriteLine(result);
            }
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson)
                .Where(p => p.Name != null && p.Name.Length >= 3 && p.Name.Length <= 15).ToList();

            context.Products.AddRange(products);

            var countOfAddedProducts = context.SaveChanges();

            return $"Successfully imported {countOfAddedProducts}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson)
                .Where(u => u.LastName != null && u.LastName.Length >= 3).ToList();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}