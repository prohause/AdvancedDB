using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static string Result = "Successfully imported {0}";

        public static void Main(string[] args)
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });

            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            var inputData = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\XML\ProductShop\ProductShop\Datasets\products.xml");

            var output = ImportProducts(context, inputData);
            Console.WriteLine(output);
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportProductDto>), new XmlRootAttribute("Products"));

            var productsDto = (List<ImportProductDto>)serializer.Deserialize(new StringReader(inputXml));

            var products = new List<Product>();

            foreach (var importProductDto in productsDto)
            {
                var product = Mapper.Map<Product>(importProductDto);
                products.Add(product);
            }

            context.AddRange(products);
            var count = context.SaveChanges();

            return string.Format(Result, count);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportUserDto>), new XmlRootAttribute("Users"));

            var usersDto = (List<ImportUserDto>)serializer.Deserialize(new StringReader(inputXml));

            var users = new List<User>();

            foreach (var importUserDto in usersDto)
            {
                var user = Mapper.Map<User>(importUserDto);
                users.Add(user);
            }

            context.AddRange(users);
            var count = context.SaveChanges();

            return string.Format(Result, count);
        }
    }
}