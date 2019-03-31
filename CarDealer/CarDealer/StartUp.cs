using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
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
            ContractResolver = ContractResolver,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                Mapper.Initialize(cfg => cfg.AddProfile(new CarDealerProfile()));

                var input = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\CarDealer\CarDealer\Datasets\customers.json");

                var output = ImportCustomers(context, input);
                Console.WriteLine(output);
            }
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