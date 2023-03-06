namespace Application.CategoryItems.Commands.CreateCategoryItem;

public record CreateCategoryItemCommand(
	string Name,
	IEnumerable<Product> Products) : IRequest<Guid>;

public record CreateCategoryItemCommandHandler : IRequestHandler<CreateCategoryItemCommand, Guid>
{
	public Task<Guid> Handle(CreateCategoryItemCommand request, CancellationToken cancellationToken)
	{
		/*
		 * Bitoque de frango
		 * * Frango grelhado
		 * * Batatatas fritas
		 * * Ovo estrelado
		 */

		return Task.FromResult(Guid.Empty);
	}
}
