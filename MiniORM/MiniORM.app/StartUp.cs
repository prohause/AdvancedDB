using MiniORM.app.Data;
using MiniORM.app.Data.Entities;
using System.Linq;

namespace MiniORM.app
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            const string connectionString = "Server=localhost\\SQLEXPRESS;Database=MiniORM;Trusted_Connection=True;";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            });

            var employee = context.Employees.Last();
            employee.FirstName = "Modified";
            context.SaveChanges();
        }
    }
}