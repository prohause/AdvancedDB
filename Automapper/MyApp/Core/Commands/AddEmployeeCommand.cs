using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class AddEmployeeCommand : ICommand
    {
        private readonly MyAppContext _context;
        private readonly Mapper _mapper;

        public AddEmployeeCommand(MyAppContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            //_context.Database.EnsureCreated();
            var firstName = inputArgs[0];
            var lastName = inputArgs[1];
            var salary = decimal.Parse(inputArgs[2]);

            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            _context.Add(employee);
            _context.SaveChanges();

            var employeeDto = _mapper.CreateMappedObject<EmployeeDto>(employee);

            var result = $"Registered successfully: {employeeDto.FirstName} {employeeDto.LastName} {employeeDto.Salary}";

            return result;
        }
    }
}