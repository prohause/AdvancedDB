using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class ManagerInfoCommand : ICommand
    {
        private readonly MyAppContext _context;
        private readonly Mapper _mapper;

        public ManagerInfoCommand(MyAppContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            var managerId = int.Parse(inputArgs[0]);

            var manager = _context.Employees.Include(x => x.ManagedEmployees).FirstOrDefault(m => m.Id == managerId);

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(managerId), "Id not found");
            }

            var managerDto = _mapper.CreateMappedObject<ManagerDto>(manager);

            var result = new StringBuilder();
            result.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.ManagedEmployees.Count}");
            managerDto.ManagedEmployees.ForEach(e => result.AppendLine($"    - {e.FirstName} {e.LastName} - ${e.Salary:F2}"));

            return result.ToString().TrimEnd();
        }
    }
}