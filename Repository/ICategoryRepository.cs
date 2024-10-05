using TestWeb.Models;
using System.Collections.Generic;

namespace TestWeb.Repository
{
    public interface ICategoryRepository
    {
        Category Add(Category category);
        Category Update(Category category);
        Category Delete(int categoryId);
        Category GetCategory(int categoryId);
        IEnumerable<Category> GetAllCategories();
    }
}
