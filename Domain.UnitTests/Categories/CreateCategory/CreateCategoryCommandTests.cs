using Application.Categories.Commands.CreateCategory;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.IntegrationTests.Categories.CreateCategory;

[Trait("Category", nameof(CreateCategoryCommand))]
public class CreateCategoryCommandTests
{
	private readonly Category _expectedCategory = new()
	{
		Id = Guid.NewGuid(),
		Name = "Bacalhau",
		ParentCategoryId = Guid.NewGuid(),
	};

	private readonly CreateCategoryCommandHandler _handler;

	private readonly CategoriesRepository _repository = new();

	public CreateCategoryCommandTests()
	{
		_handler = new(
			_repository
		);
	}

	[Fact]
	public async Task Should_CreateValidCategory()
	{
		// Arrange
		_expectedCategory.ParentCategoryId = null;

		var command = new CreateCategoryCommand(_expectedCategory.Name, _expectedCategory.ParentCategoryId);

		// Act
		var entityId = await _handler.Handle(command, default);

		// Assert
		var createdCategory = await _repository.GetById(entityId);

		Assert.NotNull(createdCategory);
		Assert.Equal(_expectedCategory.Name, createdCategory!.Name);
		Assert.Equal(_expectedCategory.ParentCategoryId, createdCategory!.ParentCategoryId);
	}

	[Fact]
	public async Task Should_ThrowNotFoundException()
	{
		var command = new CreateCategoryCommand(_expectedCategory.Name, _expectedCategory.ParentCategoryId);

		var exception
			= await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, default));

		Assert.Equal(
			$"Entity \"Category\" ({_expectedCategory.ParentCategoryId}) was not found.",
			exception.Message);
	}
}