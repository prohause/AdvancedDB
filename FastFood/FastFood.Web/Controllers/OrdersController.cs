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
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = context.Items.Select(x => x.Name).OrderBy(s => s).ToList(),
                Employees = context.Employees.Select(x => x.Name).OrderBy(s => s).ToList(),
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

            var order = mapper.Map<Order>(model);
            order.DateTime = DateTime.Now;
            var item = context.Items.FirstOrDefault(x => x.Name == model.ItemName);

            var employee = context.Employees.FirstOrDefault(x => x.Name == model.EmployeeName);
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

            context.Orders.Add(order);

            context.SaveChanges();

            return RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = context.Orders
                .ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return View(orders);
        }
    }
}