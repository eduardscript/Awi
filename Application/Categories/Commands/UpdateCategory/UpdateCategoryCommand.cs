using Application.Common.Exceptions;
using Application.Repositories;

namespace Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
	Guid Id,
	string Name,
	Guid? ParentCategoryId
) : IRequest<Category>;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category>
{
	private readonly ICategoryRepository _categoryRepository;

	public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<Category> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
	{
		var existingCategory = await _categoryRepository.GetById(request.Id);

		if (existingCategory is null)
		{
			throw new NotFoundException(nameof(Category), request.Id);
		}

		existingCategory.Name = request.Name;
		existingCategory.ParentCategoryId = request.ParentCategoryId;

		await _categoryRepository.Update(existingCategory);

		return existingCategory;
	}
}