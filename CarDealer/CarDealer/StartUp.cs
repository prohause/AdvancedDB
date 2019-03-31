using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static string Result = "Successfully imported {0}.";

        public static DefaultContractResolver ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            //ContractResolver = ContractResolver,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                Mapper.Initialize(cfg => cfg.AddProfile(new CarDealerProfile()));

                //var input = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\CarDealer\CarDealer\Datasets\sales.json");

                var output = GetSalesWithAppliedDiscount(context);
                Console.WriteLine(output);
            }
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Include(c => c.Car).ThenInclude(cp => cp.PartCars).ThenInclude(p => p.Part)
                .Include(c => c.Customer)
                .Take(10)
                .Select(x => new
                {
                    car = new
                    {
                        x.Car.Make,
                        x.Car.Model,
                        x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("0.00"),
                    price = x.Car.PartCars.Sum(p => p.Part.Price).ToString("0.00"),
                    priceWithDiscount = (x.Car.PartCars.Sum(p => p.Part.Price) * ((100 - x.Discount) / 100)).ToString("0.00")
                })
                .ToList();

            var result = JsonConvert.SerializeObject(sales, JsonSerializerSettings);
            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Include(s => s.Sales)
                .ThenInclude(c => c.Car)
                .ThenInclude(cp => cp.PartCars)
                .ThenInclude(p => p.Part)
                .Where(c => c.Sales.Any())
                .Select(x => new
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Sum(s => s.Car.PartCars.Sum(p => p.Part.Price))
                })
                .ToList();

            var result = JsonConvert.SerializeObject(customers, JsonSerializerSettings);
            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsAndParts = context.Cars.Include(c => c.PartCars).ThenInclude(cp => cp.Part)
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model
                        ,
                        c.TravelledDistance
                    },
                    parts = c.PartCars.Select(p => new
                    {
                        p.Part.Name,
                        Price = p.Part.Price.ToString("0.00")
                    })
                })
                .ToList();

            var result = JsonConvert.SerializeObject(carsAndParts, JsonSerializerSettings);

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers.Where(s => !s.IsImporter).Select(s => new
            {
                s.Id,
                s.Name,
                PartsCount = s.Parts.Count
            }).ToList();

            var result = JsonConvert.SerializeObject(suppliers, JsonSerializerSettings);

            return result;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars.Where(x => x.Make == "Toyota").OrderBy(x => x.Model).ThenByDescending(x => x.TravelledDistance).Select(x => new
            {
                x.Id,
                x.Make,
                x.Model,
                x.TravelledDistance
            }).ToList();

            var result = JsonConvert.SerializeObject(toyotaCars, JsonSerializerSettings);

            return result;
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    x.IsYoungDriver
                })

                .ToList();
            var result = JsonConvert.SerializeObject(customers, JsonSerializerSettings);

            return result;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.AddRange(sales);

            var count = context.SaveChanges();

            return string.Format(Result, count);
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.AddRange(customers);

            var count = context.SaveChanges();

            return string.Format(Result, count);
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<List<CarDto>>(inputJson);
            var carsToAdd = new List<Car>();

            foreach (var carDto in cars)
            {
                var vehicle = Mapper.Map<CarDto, Car>(carDto);

                var parts = carDto.PartsId.Distinct().ToList();

                foreach (var part in parts)
                {
                    vehicle.PartCars.Add(new PartCar { Car = vehicle, PartId = part });
                }

                carsToAdd.Add(vehicle);
            }

            context.AddRange(carsToAdd);

            var count = carsToAdd.Count;
            context.SaveChanges();

            return string.Format(Result, count);
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliers = context.Suppliers.Select(x => x.Id).ToList();

            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson).Where(x => suppliers.Contains(x.SupplierId));

            context.AddRange(parts);

            var count = context.SaveChanges();

            return string.Format(Result, count);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);
            context.AddRange(suppliers);

            var count = context.SaveChanges();

            return string.Format(Result, count);
        }

        //public class CarDto
        //{
        //    public CarDto()
        //    {
        //        PartsId = new List<int>();
        //    }

        //    public string Make { get; set; }

        //    public string Model { get; set; }

        //    public long TravelledDistance { get; set; }

        //    public List<int> PartsId { get; set; }
        //}
    }
}