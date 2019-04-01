using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static string Result = "Successfully imported {0}";

        public static XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

        public static void Main(string[] args)
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });

            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var inputData = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\XML\ProductShop\ProductShop\Datasets\categories-products.xml");

            var output = GetCategoriesByProductsCount(context);
            Console.WriteLine(output);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var sb = new StringBuilder();

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories
                .Select(c => new ExportCategoryByProductCount
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).Average(),
                    TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum()
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportCategoryByProductCount>), new XmlRootAttribute("Categories"));

            serializer.Serialize(new StringWriter(sb), categories, Namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var users = context.Users
                .Include(p => p.ProductsSold)
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new ExportSoldProductsUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new ExportSoldProductsDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToList()
                })
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportSoldProductsUserDto>), new XmlRootAttribute("Users"));

            serializer.Serialize(new StringWriter(sb), users, Namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(x => new ExportProductsInRangeDto
                {
                    Price = x.Price,
                    Name = x.Name,
                    BuyerFullName = x.Buyer.FirstName + " " + x.Buyer.LastName
                })
                .ToList();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(List<ExportProductsInRangeDto>), new XmlRootAttribute("Products"));

            serializer.Serialize(new StringWriter(sb), products, Namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCategoryProductDto>), new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDto = (List<ImportCategoryProductDto>)serializer.Deserialize(new StringReader(inputXml));

            var categoryProducts = new List<CategoryProduct>();

            var categories = context.Categories.Select(x => x.Id).ToList();

            var products = context.Products.Select(x => x.Id).ToList();

            foreach (var importCategoryProductDto in categoryProductsDto)
            {
                var categoryProduct = Mapper.Map<CategoryProduct>(importCategoryProductDto);

                if (categories.Contains(categoryProduct.CategoryId) && products.Contains(categoryProduct.ProductId))
                {
                    categoryProducts.Add(categoryProduct);
                }
            }

            context.AddRange(categoryProducts);
            context.SaveChanges();

            return string.Format(Result, categoryProducts.Count);
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCategoryDto>), new XmlRootAttribute("Categories"));

            var categoriesDto = (List<ImportCategoryDto>)serializer.Deserialize(new StringReader(inputXml));

            var categories = new List<Category>();

            foreach (var importCategoryDto in categoriesDto)
            {
                var category = Mapper.Map<Category>(importCategoryDto);
                categories.Add(category);
            }

            context.AddRange(categories);
            var count = context.SaveChanges();

            return string.Format(Result, count);
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