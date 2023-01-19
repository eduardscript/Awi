using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Categories.Common;
using Application.Common.Exceptions;
using Application.Repositories;

namespace Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand (
    Guid Id,
    string Name,
    Guid? ParentCategoryId
) : IRequest<Category>;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Category> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // TODO: uncomment this
        /*if (!await _categoryRepository.CheckCategoryExistence(request.Id))
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }*/

        var updatedCategory = new Category()
        {
            Id = request.Id,
            Name = request.Name,
            ParentCategoryId = request.ParentCategoryId
        };

        await _categoryRepository.Update(updatedCategory);

        return updatedCategory;
    }
}
