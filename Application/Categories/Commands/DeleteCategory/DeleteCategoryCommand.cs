using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(
    Guid Id
) : IRequest;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetById(request.Id);

        if (existingCategory is null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        await _categoryRepository.Delete(existingCategory);

        return Unit.Value;
    }
}
