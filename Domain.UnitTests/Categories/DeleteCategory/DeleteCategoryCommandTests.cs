using Application.Categories.Commands.DeleteCategory;
using Domain.Entities;

namespace Application.IntegrationTests.Categories.DeleteCategory;

[Trait("Category", nameof(DeleteCategoryCommand))]
public class DeleteCategoryCommandTests
{
    private readonly CategoriesRepository _repository = new();
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandTests()
    {
        _handler = new(_repository);
    }

    [Fact]
    public async Task Should_DeleteCategorySuccessfully()
    {
        var category = new Category()
        {
            Name = "Category to delete",
            ParentCategoryId = null
        };

        await _repository.Insert(category);

        var deleteCategoryCommand = new DeleteCategoryCommand(category.Id);
        await _handler.Handle(deleteCategoryCommand, default);
        
        Assert.Null(await _repository.GetById(category.Id));
    }
}