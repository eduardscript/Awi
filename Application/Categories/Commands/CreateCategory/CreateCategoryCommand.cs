﻿using Application.Common.Exceptions;
using Application.Repositories;

namespace Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
	string Name,
	Guid? ParentCategoryId
) : IRequest<Guid>;

public sealed class CreateCategoryCommandHandler
	: IRequestHandler<CreateCategoryCommand, Guid>
{
	private readonly ICategoryRepository _categoryRepository;

	public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		if (request.ParentCategoryId is not null &&
		    !await _checkCategoryExistence(request.ParentCategoryId.Value))
		{
			throw new NotFoundException(nameof(Category), request.ParentCategoryId);
		}

		var newCategory = new Category
		{
			Name = request.Name,
			ParentCategoryId = request.ParentCategoryId,
		};

		await _categoryRepository.Insert(newCategory);

		return newCategory.Id;
	}

	private async Task<bool> _checkCategoryExistence(Guid id)
	{
		var category = await _categoryRepository.GetById(id);

		return category is not null;
	}
}