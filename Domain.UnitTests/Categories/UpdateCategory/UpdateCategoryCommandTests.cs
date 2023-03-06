using Application.Categories.Commands.UpdateCategory;
using Domain.Entities;
using Application.Categories.Commands.CreateCategory;
using Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Application.IntegrationTests.Categories.UpdateCategory;

[Trait("Category", nameof(UpdateCategoryCommand))]
public class UpdateCategoryCommandTests
{
    private readonly CategoriesRepository _repository = new();
    
    private readonly UpdateCategoryCommandHandler _handler;
    
    public UpdateCategoryCommandTests()
    {
        _handler = new(_repository);
    }
    
    [Fact]
    public async Task Should_UpdateCategorySuccessfully()
    {
        var oldCategory = new Category()
        {
            Name = "Test",
            ParentCategoryId = null
        };

        await _repository.Insert(oldCategory);

        var updatedCategoryCommand = new UpdateCategoryCommand(oldCategory.Id, "Test edited", oldCategory.ParentCategoryId);

        var updatedCategory = await _handler.Handle(updatedCategoryCommand, default);
        
        Assert.NotNull(updatedCategory);
        Assert.Equal(updatedCategoryCommand.Name, updatedCategory.Name);
        Assert.NotEqual("Test", updatedCategory.Name);
    }

    [Fact]
    public async Task Should_ThrowNotFoundException()
    {
	    var updateCategoryCommand = new UpdateCategoryCommand(Guid.NewGuid(), "Test edit", null);

	    await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(updateCategoryCommand, default));
    }
}
