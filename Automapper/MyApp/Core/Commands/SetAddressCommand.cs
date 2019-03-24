using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Linq;

namespace MyApp.Core.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly MyAppContext _context;
        private readonly Mapper _mapper;

        public SetAddressCommand(MyAppContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            var employeeId = int.Parse(inputArgs[0]);
            var address = string.Join(" ", inputArgs.Skip(1));

            var employee = _context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employeeId), "Id not found in database");
            }

            employee.Address = address;
            _context.SaveChanges();

            var employeeDto = _mapper.CreateMappedObject<EmployeeDto>(employee);

            var result = $"Address changed successfully: {employeeDto.FirstName} {employeeDto.LastName}";

            return result;
        }
    }
}