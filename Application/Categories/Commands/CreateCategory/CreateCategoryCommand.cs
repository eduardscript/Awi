using Application.Common.Exceptions;
using Application.Repositories;

namespace Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
	string Name,
	Guid? ParentCategoryId
) : IRequest<Guid>;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
	private readonly ICategoryRepository _categoryRepository;

	public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		if (request.ParentCategoryId.HasValue)
		{
			var parentCategory = await _categoryRepository.CheckExistence(request.ParentCategoryId.Value);
			if (!parentCategory.Exists)
			{
				throw new NotFoundException(nameof(Category), request.ParentCategoryId.Value);
			}
		}

		var newCategory = new Category
		{
			Name = request.Name,
			ParentCategoryId = request.ParentCategoryId,
		};

		await _categoryRepository.Insert(newCategory);

		return newCategory.Id;
	}
}