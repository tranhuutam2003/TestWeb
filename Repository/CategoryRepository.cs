using System;
using System.Collections.Generic;
using System.Linq;
using TestWeb.Data;
using TestWeb.Models;

namespace TestWeb.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookContext _context;

        public CategoryRepository(BookContext context)
        {
            _context = context;
        }

        public Category Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Delete(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return category;
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return category;
        }
    }
}