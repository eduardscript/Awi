using Application.Categories.Commands.DeleteCategory;
using Application.Common.Exceptions;

namespace Application.IntegrationTests.Categories.Commands.DeleteCategory;

[Trait("Category", nameof(DeleteCategoryCommand))]
public class DeleteCategoryCommandTests
{
	private readonly DeleteCategoryCommandHandler _handler;
	private readonly CategoriesRepository _repository = new();

	public DeleteCategoryCommandTests()
	{
		_handler = new(_repository);
	}

	[Fact]
	public async Task Should_DeleteCategorySuccessfully()
	{
		// Arrange
		var category = new Category
		{
			Name = "Category to delete",
			ParentCategoryId = null,
		};

		await _repository.Insert(category);

		// Act
		var deleteCategoryCommand = new DeleteCategoryCommand(category.Id);
		await _handler.Handle(deleteCategoryCommand, default);

		// Assert
		Assert.Null(await _repository.GetById(category.Id));
	}

	[Fact]
	public async Task Should_ThrowNotFoundException()
	{
		var deleteCategoryCommand = new DeleteCategoryCommand(Guid.NewGuid());

		var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(deleteCategoryCommand, default));
		
		Assert.Equal(
			$"Entity \"Category\" ({deleteCategoryCommand.Id}) was not found.",
			exception.Message);
	}
}