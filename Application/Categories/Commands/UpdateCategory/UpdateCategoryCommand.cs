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
		var existingCategory = await _categoryRepository.CheckExistence(request.Id);

		if (!existingCategory.Exists)
		{
			throw new NotFoundException(nameof(Category), request.Id);
		}

		existingCategory.Entity!.Name = request.Name;
		
		// TODO: Check if parent category id exists and write the tests to it
		existingCategory.Entity!.ParentCategoryId = request.ParentCategoryId;

		await _categoryRepository.Update(existingCategory.Entity!);

		return existingCategory.Entity!;
	}
}