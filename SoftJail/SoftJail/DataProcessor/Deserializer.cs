using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using SoftJail.DataProcessor.ImportDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor
{
    using Data;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var importedDepartments = JsonConvert.DeserializeObject<List<ImportDepartmentDto>>(jsonString);

            var departments = new List<Department>();

            foreach (var departmentDto in importedDepartments)
            {
                if (!IsValid(departmentDto) || departmentDto.Cells.Any(x => !IsValid(x)))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var department = new Department
                {
                    Name = departmentDto.Name
                };

                department.Cells = departmentDto.Cells.Select(x => new Cell
                {
                    Department = department,
                    HasWindow = x.HasWindow,
                    CellNumber = x.CellNumber
                }).ToList();

                departments.Add(department);
                result.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.AddRange(departments);
            context.SaveChanges();

            return result.ToString().TrimStart();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var importedPrisoners = JsonConvert.DeserializeObject<List<ImportPrisonerDto>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            var prisoners = new List<Prisoner>();
            var mails = new List<Mail>();

            foreach (var dto in importedPrisoners)
            {
                if (!IsValid(dto) || dto.Mails != null && dto.Mails.Any(x => !IsValid(x)))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var prisoner = new Prisoner
                {
                    Age = dto.Age,
                    CellId = dto.CellId,
                    Bail = dto.Bail,
                    FullName = dto.FullName,
                    IncarcerationDate = dto.IncarcerationDate,
                    Nickname = dto.Nickname
                };

                foreach (var mailDto in dto.Mails)
                {
                    var mail = new Mail
                    {
                        Sender = mailDto.Sender,
                        Address = mailDto.Address,
                        Description = mailDto.Description,
                        Prisoner = prisoner
                    };

                    mails.Add(mail);
                }
                prisoners.Add(prisoner);
                result.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.AddRange(prisoners);
            context.AddRange(mails);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(List<ImportOfficerPrisonerDto>), new XmlRootAttribute("Officers"));

            var importedOfficers = (List<ImportOfficerPrisonerDto>)serializer.Deserialize(new StringReader(xmlString));

            var officers = new List<Officer>();

            foreach (var dto in importedOfficers)
            {
                var positionIsValid = Enum.TryParse<PositionType>(dto.Position, out var currentPositionType);
                var weaponIsValid = Enum.TryParse<WeaponType>(dto.Weapon, out var currentWeaponType);

                if (!IsValid(dto) || !positionIsValid || !weaponIsValid)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer
                {
                    DepartmentId = dto.DepartmentId,
                    FullName = dto.Name,
                    Position = currentPositionType,
                    Weapon = currentWeaponType,
                    Salary = dto.Money,
                    OfficerPrisoners = new List<OfficerPrisoner>()
                };

                officers.Add(officer);

                foreach (var officerDto in dto.Prisoners)
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner
                    {
                        Officer = officer,
                        PrisonerId = officerDto.Id
                    });
                }

                result.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.AddRange(officers);
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