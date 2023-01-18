using Domain.Entities.Common;

namespace Domain.Entities
{
    public class CategoryItem : BaseEntity
    {
		public string Name { get; set; } = null!;

		public List<Product>? Products { get; set; }
	}
}
