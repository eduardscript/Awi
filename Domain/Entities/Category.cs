using Domain.Entities.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
	public string Name { get; set; } = null!;

	public Guid? ParentCategoryId { get; set; }

	public List<CategoryItem>? CategoryItems { get; set; }
}