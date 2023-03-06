namespace Application.Categories.Common.Exceptions;

public class RelateCategoryException : Exception
{
	public RelateCategoryException(Category parentCategory, Category relateCategory)
		: base($"You can't relate a category with a parent category that already have items. The parent category: \"{parentCategory.Id}\" already have {parentCategory.CategoryItems!.Count} category items.")
	{ }
}