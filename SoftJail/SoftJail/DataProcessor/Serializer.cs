using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor
{
    using Data;
    using System;

    public class Serializer
    {
        public static XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners.Where(p => ids.Contains(p.Id) && p.CellId != null).Include(c => c.Cell).Include(po => po.PrisonerOfficers).ThenInclude(o => o.Officer).ThenInclude(o => o.Department).Select(p => new
            {
                p.Id,
                Name = p.FullName,
                p.Cell.CellNumber,
                Officers = p.PrisonerOfficers.Select(po => new
                {
                    OfficerName = po.Officer.FullName,
                    Department = po.Officer.Department.Name
                }).OrderBy(x => x.OfficerName),
                TotalOfficerSalary = p.PrisonerOfficers.Sum(po => po.Officer.Salary)
            })
                .OrderBy(x => x.Name)
                .ToList();
            var result = JsonConvert.SerializeObject(prisoners);
            return result;

            //"Id": 3,
            //"Name": "Binni Cornhill",
            //"CellNumber": 503,
            //"Officers": [
            //{
            //    "OfficerName": "Hailee Kennon",
            //    "Department": "ArtificialIntelligence"
            //},
            //{
            //    "OfficerName": "Theo Carde",
            //    "Department": "Blockchain"
            //}
            //    "TotalOfficerSalary": 7127.93
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var result = new StringBuilder();

            var validInmates = prisonersNames.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            var prisoners = context.Prisoners.Where(p => validInmates.Contains(p.FullName)).Include(m => m.Mails)
                .Select(p => new ExportPrisonerDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    Date = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                    MessageDtos = p.Mails == null ? new List<ExportMessageDto>() :
                        p.Mails.Select(m => new ExportMessageDto
                        {
                            Description = ReverseString(m.Description)
                        }).ToList()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ExportPrisonerDto>), new XmlRootAttribute("Prisoners"));
            serializer.Serialize(new StringWriter(result), prisoners, Namespaces);

            return result.ToString().TrimEnd();
        }

        private static string ReverseString(string argDescription)
        {
            return new string(argDescription.Reverse().ToArray());
        }
    }
}