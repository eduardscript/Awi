namespace Application.CategoryItems.Commands;

public record CreateCategoryItemCommand(
	string Name,
	IEnumerable<Product> Products) : IRequest<Guid>;