using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;

namespace MyApp.Core.Commands
{
    public class EmployeeInfoCommand : ICommand
    {
        private readonly MyAppContext _context;
        private readonly Mapper _mapper;

        public EmployeeInfoCommand(MyAppContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            var employeeId = int.Parse(inputArgs[0]);

            var employee = _context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employeeId), "Id not found in the database");
            }

            var employeeDto = _mapper.CreateMappedObject<EmployeeDto>(employee);

            var result = $"ID: {employeeId} - {employeeDto.FirstName} {employeeDto.LastName} -  ${employeeDto.Salary:F2}";

            return result;
        }
    }
}