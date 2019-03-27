using AutoMapper.QueryableExtensions;
using FastFood.Models;
using FastFood.Models.Enums;
using System;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = _context.Items.Select(x => x.Name).OrderBy(s => s).ToList(),
                Employees = _context.Employees.Select(x => x.Name).OrderBy(s => s).ToList(),
            };

            return View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var order = _mapper.Map<Order>(model);
            order.DateTime = DateTime.Now;
            var item = _context.Items.FirstOrDefault(x => x.Name == model.ItemName);

            var employee = _context.Employees.FirstOrDefault(x => x.Name == model.EmployeeName);
            if (item != null)
            {
                order.OrderItems.Add(new OrderItem()
                {
                    ItemId = item.Id,
                    Order = order,
                    Quantity = model.Quantity
                });
            }

            order.Type = Enum.Parse<OrderType>(model.OrderType);

            if (employee != null)
            {
                order.EmployeeId = employee.Id;
            }

            _context.Orders.Add(order);

            _context.SaveChanges();

            return RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = _context.Orders
                .ProjectTo<OrderAllViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            return View(orders);
        }
    }
}