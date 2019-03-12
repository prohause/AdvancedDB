using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var result = RemoveTown(context);
                Console.WriteLine(result);
            }
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var sb = new StringBuilder();
            context.Employees.Where(x => x.Address.Town.Name.Equals("Seattle")).ToList().ForEach(x => x.AddressId = null);
            var count = context.Addresses.Count(a => a.Town.Name.Equals("Seattle"));
            sb.AppendLine($"{count} addresses in Seattle were deleted");

            var addressesToRemove = context.Addresses.Where(a => a.Town.Name.Equals("Seattle")).ToList();

            context.Addresses.RemoveRange(addressesToRemove);
            var townToRemove = context.Towns.First(t => t.Name.Equals("Seattle"));
            context.Towns.Remove(townToRemove);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projectsToRemove = context.Projects.Find(2);
            var employeeProjectsToRemove = context.EmployeesProjects.Where(p => p.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeeProjectsToRemove);

            context.Projects.Remove(projectsToRemove);

            context.SaveChanges();

            var projects = context.Projects.Select(x => new
            {
                x.Name
            }).Take(10).ToList();

            projects.ForEach(x => sb.AppendLine(x.Name));

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                }).OrderBy(x => x.EmployeeId).ToList();

            employees.ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Employees.Where(e => e.Salary > 50000).Select(x => new { x.FirstName, x.Salary })
                .OrderBy(s => s.FirstName).ToList();

            collection.ForEach(e => sb.AppendLine($"{e.FirstName} - {e.Salary:F2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Employees.Where(e => e.Department.Name == "Research and Development")
                .Select(x => new { x.FirstName, x.LastName, DepartmentName = x.Department.Name, x.Salary }).OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName).ToList();

            collection.ForEach(x => sb.AppendLine($"{x.FirstName} {x.LastName} from {x.DepartmentName} - ${x.Salary:F2}"));

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var address = new Address { AddressText = "Vitoshka 15", TownId = 4 };
            context.Addresses.Add(address);

            var nakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            nakov.Address = address;

            context.SaveChanges();

            var collection = context.Employees.Select(x => new { x.AddressId, x.Address.AddressText })
                .OrderByDescending(x => x.AddressId).Take(10).ToList();

            collection.ForEach(x => sb.AppendLine($"{x.AddressText}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Employees.Where(e =>
                    e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFullName = $"{x.FirstName} {x.LastName}",
                    ManagerFullName = $"{x.Manager.FirstName} {x.Manager.LastName}",
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        p.Project.StartDate,
                        p.Project.EndDate
                    }).ToList()
                }).Take(10).ToList();

            foreach (var employee in collection)
            {
                sb.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

                foreach (var employeeProject in employee.Projects)
                {
                    var endDate = employeeProject.EndDate;
                    var endDateAsString = endDate != null ? ((DateTime)endDate).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";

                    sb.AppendLine(
                        $"--{employeeProject.ProjectName} - {employeeProject.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDateAsString}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Addresses.Select(x => new
            {
                x.AddressText,
                TownName = x.Town.Name,
                EmployeeCount = x.Employees.Count
            }).OrderByDescending(a => a.EmployeeCount).ThenBy(a => a.TownName).ThenBy(a => a.AddressText).Take(10).ToList();

            collection.ForEach(a => sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees"));

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Employees.Where(e => e.EmployeeId == 147).Select(x => new
            {
                FullName = x.FirstName + " " + x.LastName,
                x.JobTitle,
                Projects = x.EmployeesProjects.Select(p => new
                {
                    ProjectName = p.Project.Name
                }).OrderBy(p => p.ProjectName).ToList()
            }).ToList();

            var result = collection.First();

            sb.AppendLine($"{result.FullName} - {result.JobTitle}");
            result.Projects.ForEach(p => sb.AppendLine(p.ProjectName));

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Departments.Where(d => d.Employees.Count > 5).OrderBy(x => x.Employees.Count).ThenBy(x => x.Name).Select(x => new
            {
                DepartmentName = x.Name,
                ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                Employees = x.Employees.Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle
                }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
            }).ToList();

            foreach (var department in collection)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerName}");
                department.Employees.ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}"));
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Projects.OrderByDescending(p => p.StartDate).Select(x => new
            {
                x.Name,
                x.Description,
                x.StartDate
            }).Take(10).OrderBy(p => p.Name).ToList();

            foreach (var project in collection)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var neededDepartments = new List<string> { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var collection = context.Employees.Where(e => neededDepartments.Contains(e.Department.Name)).ToList();
            collection.ForEach(e => e.Salary *= 1.12m);
            context.SaveChanges();

            var result = context.Employees.Where(e => neededDepartments.Contains(e.Department.Name)).Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Salary
            }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();

            result.ForEach(e => sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})"));

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var collection = context.Employees.Where(e => e.FirstName.StartsWith("Sa")).OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName).Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                }).ToList();

            collection.ForEach(x => sb.AppendLine($"{x.FirstName} {x.LastName} - {x.JobTitle} - (${x.Salary:F2})"));
            return sb.ToString().TrimEnd();
        }
    }
}