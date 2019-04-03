using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static string Result = "Successfully imported {0}";

        public static XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

        public static void Main(string[] args)
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<CarDealerProfile>();
            });

            //var inputData = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\XML\CarDealer\CarDealer\Datasets\sales.xml");

            using (var context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                var output = GetLocalSuppliers(context);
                Console.WriteLine(output);
            }
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new ExportSupplierListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();

            var output = new ExportAllSuppliersDto
            {
                ExportSupplierListDtos = suppliers
            };

            var serializer = new XmlSerializer(typeof(ExportAllSuppliersDto), new XmlRootAttribute("suppliers"));

            serializer.Serialize(new StringWriter(sb), output, Namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Model,
                    c.TravelledDistance
                })
                .ToList();

            var output = new ExportBwmMakeDto
            {
                Car = cars.Select(x => new ExportSingleCasDto
                {
                    Id = x.Id,
                    Model = x.Model,
                    Travelleddistance = x.TravelledDistance
                }).ToList()
            };

            var serializer = new XmlSerializer(typeof(ExportBwmMakeDto), new XmlRootAttribute("cars"));

            serializer.Serialize(new StringWriter(sb), output, Namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var sb = new StringBuilder();

            var cars = context.Cars
                .Where(c => c.TravelledDistance >= 2000000)
                .Select(x => new ExportCarDto
                {
                    Make = x.Make,
                    Model = x.Model,
                    Distance = x.TravelledDistance
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportCarDto>), new XmlRootAttribute("cars"));

            serializer.Serialize(new StringWriter(sb), cars, Namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var carIds = context.Cars.Select(x => x.Id);

            var serializer = new XmlSerializer(typeof(List<ImportSaleDto>), new XmlRootAttribute("Sales"));

            var saleDtos = (List<ImportSaleDto>)serializer.Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();

            foreach (var saleDto in saleDtos)
            {
                if (carIds.Contains(saleDto.CarId))
                {
                    sales.Add(Mapper.Map<Sale>(saleDto));
                }
            }

            context.AddRange(sales);

            var count = context.SaveChanges();

            return string.Format(Result, count);
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCustomerDto>), new XmlRootAttribute("Customers"));

            var customersDto = (List<ImportCustomerDto>)serializer.Deserialize(new StringReader(inputXml));

            var customers = new List<Customer>();

            customersDto.ForEach(x => customers.Add(Mapper.Map<Customer>(x)));

            context.AddRange(customers);

            var count = context.SaveChanges();

            return string.Format(Result, count);
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var partIds = context.Parts.Select(p => p.Id).ToList();

            var serializer = new XmlSerializer(typeof(List<ImportCarDto>), new XmlRootAttribute("Cars"));

            var carDtos = (List<ImportCarDto>)serializer.Deserialize(new StringReader(inputXml));

            var partCars = new List<PartCar>();

            foreach (var carDto in carDtos)
            {
                var vehicle = Mapper.Map<Car>(carDto);
                partCars.AddRange(from partDto in carDto.ImportPartsListDto.Select(p => p.Id).Distinct() where partIds.Contains(partDto) select new PartCar { PartId = partDto, Car = vehicle });
            }

            context.AddRange(partCars);
            context.SaveChanges();

            var count = context.Cars.Count();

            return string.Format(Result, count);
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var suppliersIds = context.Suppliers.Select(x => x.Id).ToList();

            var serializer = new XmlSerializer(typeof(List<ImportPartDto>), new XmlRootAttribute("Parts"));

            var partsDtos = (List<ImportPartDto>)serializer.Deserialize(new StringReader(inputXml));

            var parts = new List<Part>();

            partsDtos.Where(p => suppliersIds.Contains(p.SupplierId)).ToList().ForEach(x => parts.Add(Mapper.Map<Part>(x)));

            context.AddRange(parts);

            var count = context.SaveChanges();
            return string.Format(Result, count);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportSupplierDto>), new XmlRootAttribute("Suppliers"));

            var suppliersDtos = (List<ImportSupplierDto>)serializer.Deserialize(new StringReader(inputXml));

            var suppliers = new List<Supplier>();

            suppliersDtos.ForEach(x => suppliers.Add(Mapper.Map<Supplier>(x)));

            context.AddRange(suppliers);

            var count = context.SaveChanges();

            return string.Format(Result, count);
        }
    }
}