using AutoMapper.QueryableExtensions;
using FastFood.Models;
using System.Linq;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public EmployeesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Register()
        {
            var positions = context
                .Positions
                .ProjectTo<RegisterEmployeeViewModel>(mapper.ConfigurationProvider)
                .OrderBy(p => p.PositionName).ToList();

            return View(positions);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var employee = mapper.Map<Employee>(model);

            var position = context.Positions.FirstOrDefault(x => x.Name == model.PositionName);
            if (position != null)
            {
                employee.PositionId = position.Id;
            }

            context.Employees.Add(employee);
            context.SaveChanges();

            return RedirectToAction("All", "Employees");
        }

        public IActionResult All()
        {
            var employees = context.Employees
                .ProjectTo<EmployeesAllViewModel>(mapper.ConfigurationProvider).ToList();

            return View(employees);
        }
    }
}