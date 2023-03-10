using Application.Categories.Commands.UpdateCategory;
using Application.Common.Exceptions;

namespace Application.IntegrationTests.Categories.Commands.UpdateCategory;

[Trait("Category", nameof(UpdateCategoryCommand))]
public class UpdateCategoryCommandTests
{
	private readonly UpdateCategoryCommandHandler _handler;
	private readonly CategoriesRepository _repository = new();

	public UpdateCategoryCommandTests()
	{
		_handler = new(_repository);
	}

	[Fact]
	public async Task Should_UpdateCategorySuccessfully()
	{
		var oldCategory = new Category
		{
			Name = "Test",
			ParentCategoryId = null,
		};

		await _repository.Insert(oldCategory);

		var updatedCategoryCommand = new UpdateCategoryCommand(
			oldCategory.Id,
			"Test edited",
			oldCategory.ParentCategoryId);

		var updatedCategory = await _handler.Handle(updatedCategoryCommand, default);

		Assert.NotNull(updatedCategory);
		Assert.Equal(updatedCategoryCommand.Name, updatedCategory.Name);
		Assert.NotEqual("Test", updatedCategory.Name);
	}

	[Fact]
	public async Task Should_ThrowNotFoundException()
	{
		// Arrange
		var updateCategoryCommand = new UpdateCategoryCommand(Guid.NewGuid(), "Test edit", null);

		// Act & Assert
		await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(updateCategoryCommand, default));
	}
}