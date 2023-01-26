using Domain.Entities.Common;

namespace Domain.Entities;

public class Product : BaseEntity
{
	public string Name { get; set; } = null!;
}