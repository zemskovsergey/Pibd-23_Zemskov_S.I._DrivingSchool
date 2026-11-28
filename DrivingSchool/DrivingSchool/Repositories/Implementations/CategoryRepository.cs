using System;
using System.Collections.Generic;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;
using Serilog;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class CategoryRepository : EfRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(Func<Owned<DataContext>> openContext, ILogger logger)
            : base(openContext, logger, nameof(Category.CategoryId), category => category.CategoryId)
        {
        }

        public IEnumerable<Category> ReadCategories() => ReadAll(nameof(ReadCategories));

        public Category ReadCategoryById(int id) => ReadById(nameof(ReadCategoryById), id);

        public void CreateCategory(Category category) => Create(nameof(CreateCategory), category);

        public void UpdateCategory(Category category) =>
            Update(nameof(UpdateCategory), category, (context, existing, values) =>
                context.Entry(existing).Property(x => x.Name).CurrentValue = values.Name);

        public void DeleteCategory(int id) => Delete(nameof(DeleteCategory), id);
    }
}
