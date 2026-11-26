using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> ReadCategories();
        Category ReadCategoryById(int id);
        void CreateCategory(Category category);
        void DeleteCategory(int id);
    }
}