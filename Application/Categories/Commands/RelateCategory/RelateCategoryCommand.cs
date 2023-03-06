using Application.Categories.Common.Exceptions;
using Application.Common.Exceptions;
using Application.Repositories;

namespace Application.Categories.Commands.RelateCategory;

public record RelateCategoryCommand(Guid ParentCategoryId, Guid RelateCategoryId) : IRequest;

public sealed class RelateCategoryCommandHandler : IRequestHandler<RelateCategoryCommand>
{
	private readonly ICategoryRepository _categoryRepository;

	public RelateCategoryCommandHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<Unit> Handle(RelateCategoryCommand request, CancellationToken cancellationToken)
	{
		var parentCategory = await _categoryRepository.CheckExistence(request.ParentCategoryId);
		if (!parentCategory.Exists)
		{
			throw new NotFoundException(nameof(Category), request.ParentCategoryId);
		}

		var relateCategory = await _categoryRepository.CheckExistence(request.RelateCategoryId);
		if (!relateCategory.Exists)
		{
			throw new NotFoundException(nameof(Category), request.RelateCategoryId);
		}
		
		if ((bool)parentCategory.Entity?.CategoryItems?.Any())
		{
			throw new RelateCategoryException(parentCategory.Entity!, relateCategory.Entity!);
		}

		relateCategory.Entity!.ParentCategoryId = request.ParentCategoryId;

		await _categoryRepository.Update(relateCategory.Entity);
		
		return Unit.Value;
	}
}