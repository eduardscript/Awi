using Application.Categories.Commands.RelateCategory;
using Application.Categories.Common.Exceptions;
using Application.Common.Exceptions;

namespace Application.IntegrationTests.Categories.Commands.RelateCategory;

[Trait("Category", nameof(RelateCategoryCommandTests))]
public class RelateCategoryCommandTests
{
	private readonly RelateCategoryCommandHandler _handler;
	private readonly CategoriesRepository _repository = new();

	public RelateCategoryCommandTests()
	{
		_handler = new(_repository);
	}

	[Fact]
	public async Task InvalidParentCategory_Should_ThrowNotFoundException()
	{
		// Arrange
		var category = new Category
		{
			Id = Guid.NewGuid(),
			Name = "Test",
			ParentCategoryId = Guid.NewGuid(),
		};

		await _repository.Insert(category);

		// Act
		var relateCategoryCommand = new RelateCategoryCommand(
			category.ParentCategoryId.Value,
			category.Id
		);

		var exception = await Assert.ThrowsAsync<NotFoundException>(
			async () => await _handler.Handle(relateCategoryCommand, default)
		);

		// Assert
		Assert.Equal(
			$"Entity \"Category\" ({relateCategoryCommand.ParentCategoryId}) was not found.",
			exception.Message);
	}

	[Fact]
	public async Task InvalidRelateCategory_Should_ThrowNotFoundException()
	{
		// Arrange
		var parentCategory = new Category
		{
			Id = Guid.NewGuid(),
			Name = "Test",
		};

		await _repository.Insert(parentCategory);

		var relateCategory = new Category
		{
			Id = Guid.NewGuid(),
			ParentCategoryId = parentCategory.Id,
			Name = "Test",
		};

		// Act
		var relateCategoryCommand = new RelateCategoryCommand(
			parentCategory.Id,
			relateCategory.Id
		);

		var exception = await Assert.ThrowsAsync<NotFoundException>(
			async () => await _handler.Handle(relateCategoryCommand, default)
		);

		// Assert
		Assert.Equal(
			$"Entity \"Category\" ({relateCategoryCommand.RelateCategoryId}) was not found.",
			exception.Message);
	}

	[Fact]
	public async Task ParentCategoryWithItems_Should_ThrowRelateCategoryException()
	{
		// Arrange
		var parentCategory = new Category
		{
			Id = Guid.NewGuid(),
			Name = "Test",
			CategoryItems = new()
			{
				new()
				{
					Id = Guid.NewGuid(),
					Name = "Bitoque de frango", Products = new()
					{
						new() { Id = Guid.NewGuid(), Name = "Frango", },
						new() { Id = Guid.NewGuid(), Name = "Batatas Fritas", },
						new() { Id = Guid.NewGuid(), Name = "Ovo estrelado", },
					},
				},
			},
		};

		await _repository.Insert(parentCategory);

		var relateCategory = new Category
		{
			Id = Guid.NewGuid(),
			ParentCategoryId = parentCategory.Id,
			Name = "Test",
		};

		await _repository.Insert(relateCategory);

		// Act
		var relateCategoryCommand = new RelateCategoryCommand(
			parentCategory.Id,
			relateCategory.Id
		);

		var exception = await Assert.ThrowsAsync<RelateCategoryException>(
			async () => await _handler.Handle(relateCategoryCommand, default)
		);

		// Assert
		Assert.Equal(
			$"You can't relate a category with a parent category that already have items. The parent category: \"{parentCategory.Id}\" already have {parentCategory.CategoryItems!.Count} category items.",
			exception.Message);
	}
	
	[Fact]
	public async Task ParentCategoryWithoutItems_Should_UpdateRelateCategory()
	{
		// Arrange
		var parentCategory = new Category
		{
			Id = Guid.NewGuid(),
			Name = "Test",
		};

		await _repository.Insert(parentCategory);

		var relateCategory = new Category
		{
			Id = Guid.NewGuid(),
			Name = "Test",
			CategoryItems = new()
			{
				new()
				{
					Id = Guid.NewGuid(),
					Name = "Bitoque de frango", Products = new()
					{
						new() { Id = Guid.NewGuid(), Name = "Frango", },
						new() { Id = Guid.NewGuid(), Name = "Batatas Fritas", },
						new() { Id = Guid.NewGuid(), Name = "Ovo estrelado", },
					},
				},
			},
			ParentCategoryId = null!,
		};

		await _repository.Insert(relateCategory);

		// Act
		var relateCategoryCommand = new RelateCategoryCommand(
			parentCategory.Id,
			relateCategory.Id
		);

		var updatedRelateCategory = await _repository.GetById(relateCategoryCommand.RelateCategoryId);
		
		// Assert
		Assert.NotNull(updatedRelateCategory);
		Assert.Equal(relateCategory.Name, updatedRelateCategory.Name);
		Assert.Equal(relateCategory.Id, updatedRelateCategory.Id);
		Assert.Equal(parentCategory.ParentCategoryId, updatedRelateCategory.ParentCategoryId);
	}
}