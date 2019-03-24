using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;
using System;
using System.Linq;
using System.Text;

namespace MyApp.Core.Commands
{
    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly MyAppContext _context;
        private readonly Mapper _mapper;

        public ListEmployeesOlderThanCommand(MyAppContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            var age = int.Parse(inputArgs[0]);

            var neededYear = DateTime.Now.AddYears(age * -1);

            var employees = _context.Employees.Where(x => x.Birthday <= neededYear).Include(m => m.Manager).ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine(
                    $"{employee.FirstName} {employee.LastName} - ${employee.Salary:F2} - Manager: " +
                    $"{(employee.Manager == null ? "[no manager]" : employee.Manager?.FirstName)}");
            }

            return result.ToString().TrimEnd();
        }
    }
}