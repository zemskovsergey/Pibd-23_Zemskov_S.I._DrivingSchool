using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class CategoryRepositoryMock : ICategoryRepository
    {
        private readonly List<Category> _categories = new List<Category>();

        public IEnumerable<Category> ReadCategories() => _categories;

        public Category ReadCategoryById(int id) => _categories.FirstOrDefault(c => c.CategoryId == id);

        public void CreateCategory(Category category)
        {
            _categories.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            var existing = ReadCategoryById(category.CategoryId);
            if (existing != null)
            {
                _categories.Remove(existing);
                _categories.Add(category);
            }
        }

        public void DeleteCategory(int id)
        {
            var category = ReadCategoryById(id);
            if (category != null)
            {
                _categories.Remove(category);
            }
        }
    }
}