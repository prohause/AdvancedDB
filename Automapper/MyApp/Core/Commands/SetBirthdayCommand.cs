﻿using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using System;
using System.Globalization;

namespace MyApp.Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly MyAppContext _context;
        private readonly Mapper _mapper;

        public SetBirthdayCommand(MyAppContext context, Mapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            var employeeId = int.Parse(inputArgs[0]);
            var birthDate = DateTime.ParseExact(inputArgs[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var employee = _context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employeeId), "Id not found in the database");
            }

            employee.Birthday = birthDate;
            _context.SaveChanges();

            var employeeDto = _mapper.CreateMappedObject<EmployeeDto>(employee);

            var result = $"Birthday changed successfully: {employeeDto.FirstName} {employeeDto.LastName}";

            return result;
        }
    }
}