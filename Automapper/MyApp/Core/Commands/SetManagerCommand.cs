using MyApp.Core.Commands.Contracts;
using MyApp.Data;
using System;

namespace MyApp.Core.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly MyAppContext _context;

        public SetManagerCommand(MyAppContext context)
        {
            _context = context;
        }

        public string Execute(string[] inputArgs)
        {
            var employeeId = int.Parse(inputArgs[0]);
            var managerId = int.Parse(inputArgs[1]);

            var employee = _context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employeeId), "No employee found with this id");
            }

            var manager = _context.Employees.Find(managerId);

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(managerId));
            }

            employee.Manager = manager;

            _context.SaveChanges();

            return "Manager changed successfully";
        }
    }
}