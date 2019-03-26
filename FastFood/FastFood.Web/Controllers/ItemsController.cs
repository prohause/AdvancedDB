using FastFood.Models;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public ItemsController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var categories = context.Categories
                .ProjectTo<CreateItemViewModel>(mapper.ConfigurationProvider)
                .OrderBy(x => x.CategoryName)
                .ToList();

            return View(categories);
        }

        [HttpPost]
        public IActionResult Create(CreateItemInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var item = mapper.Map<Item>(model);

            var category = context.Categories.FirstOrDefault(x => x.Name == model.CategoryName);

            if (category != null)
            {
                item.CategoryId = category.Id;
            }

            context.Items.Add(item);
            context.SaveChanges();

            return RedirectToAction("All", "Items");
        }

        public IActionResult All()
        {
            var items = context.Items
                .ProjectTo<ItemsAllViewModels>(mapper.ConfigurationProvider).ToList();

            return View(items);
        }
    }
}