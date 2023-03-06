using Application.Common.Exceptions;
using Application.Repositories;

namespace Application.Categories.Queries.GetCategoriesWithPagination;

public record GetCategoriesWithPaginationQuery(Guid CategoryId) : IRequest<CategoryDto>;

public sealed class GetCategoriesWithPaginationQueryHandler
	: IRequestHandler<GetCategoriesWithPaginationQuery, CategoryDto>
{
	private readonly ICategoryRepository _categoryRepository;

	public GetCategoriesWithPaginationQueryHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<CategoryDto> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
	{
		var existingCategory = await _categoryRepository.GetById(request.CategoryId);

		if (existingCategory is null)
		{
			throw new NotFoundException(nameof(Category), request.CategoryId);
		}

		return new(existingCategory.Id, existingCategory.Name, existingCategory.ParentCategoryId);
	}
}

public sealed record CategoryDto(Guid Id, string Name, Guid? ParentCategoryId);