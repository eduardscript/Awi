using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Categories.Common;
using Application.Common.Exceptions;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
	string Name,
	Guid? ParentCategoryId
) : IRequest<Guid>;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.ParentCategoryId is not null &&
            !await _categoryRepository.CheckCategoryExistence(request.ParentCategoryId.Value))
        {
            throw new NotFoundException(nameof(Category), request.ParentCategoryId);
        }

        var newCategory = new Category
        {
            Name = request.Name,
            ParentCategoryId = request.ParentCategoryId
        };

        await _categoryRepository.Insert(newCategory);

        return newCategory.Id;
    }
}

