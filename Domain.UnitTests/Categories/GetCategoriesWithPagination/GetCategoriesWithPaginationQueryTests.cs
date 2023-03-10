using Application.Categories.Queries.GetCategoriesWithPagination;
using Application.Common.Exceptions;

namespace Application.IntegrationTests.Categories.GetCategoriesWithPagination;

[Trait("Category", nameof(GetCategoriesWithPaginationQuery))]
public class GetCategoriesWithPaginationQueryTests
{
	private readonly Category _expectedCategory = new()
	{
		Id = Guid.NewGuid(),
		Name = "Bacalhau",
		ParentCategoryId = null!,
	};

	private readonly GetCategoriesWithPaginationQueryHandler _handler;

	private readonly CategoriesRepository _repository = new();

	public GetCategoriesWithPaginationQueryTests()
	{
		_handler = new(_repository);
	}

	[Fact]
	public async Task Should_GetCategory()
	{
		// Arrange
		await _repository.Insert(_expectedCategory);

		// Act
		var command = new GetCategoriesWithPaginationQuery(_expectedCategory.Id);
		var categoryDto = await _handler.Handle(command, default);

		// Assert
		Assert.Equal(_expectedCategory.Id, categoryDto.Id);
		Assert.Equal(_expectedCategory.Name, categoryDto.Name);
		Assert.Equal(_expectedCategory.ParentCategoryId, categoryDto.ParentCategoryId);
	}

	[Fact]
	public async Task Should_ThrowNotFoundException()
	{
		// Arrange
		var command = new GetCategoriesWithPaginationQuery(_expectedCategory.Id);

		// Act
		var exception
			= await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, default));

		// Assert
		Assert.Equal(
			$"Entity \"Category\" ({_expectedCategory.Id}) was not found.",
			exception.Message);
	}
}