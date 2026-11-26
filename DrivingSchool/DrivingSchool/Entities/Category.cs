
namespace DrivingSchool.Entities
{
    public class Category
    {
        public int CategoryId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public static Category CreateEntity(int categoryId, string name)
        {
            return new Category
            {
                CategoryId = categoryId,
                Name = name ?? string.Empty
            };
        }
    }
}
