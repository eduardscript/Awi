using Application.Repositories;

namespace Application.Categories.Common;

public static class CategoryMethods
{
	public static async Task<bool> CheckCategoryExistence(this ICategoryRepository categoryRepository, Guid id)
	{
		var category = await categoryRepository.GetById(id);

		return category is not null;
	}
}