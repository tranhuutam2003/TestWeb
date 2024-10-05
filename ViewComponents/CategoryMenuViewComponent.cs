using TestWeb.Models;
using Microsoft.AspNetCore.Mvc;
using TestWeb.Repository;

namespace TestWeb.ViewComponents
{
    public class CategoryMenuViewComponent:ViewComponent
    {
        private readonly ICategoryRepository _category;
        public CategoryMenuViewComponent(ICategoryRepository categoryRepository)
        {
            _category = categoryRepository;
        }
        public IViewComponentResult Invoke()
        {
            var category = _category.GetAllCategories().OrderBy(x=>x.CategoryName);
            return View(category);
        }
    }
}
