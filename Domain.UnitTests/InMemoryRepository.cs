using Application.Repositories;
using Application.Repositories.Common;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests;

internal class InMemoryRepository<TEntity> : DbContext, IGenericRepository<TEntity>
	where TEntity : BaseEntity
{
	public DbSet<Category> Categories { get; set; }

	public async Task<TEntity?> GetById(Guid id) => await Set<TEntity>()
		.FindAsync(id);

	public async Task<TEntity> Insert(TEntity entity)
	{
		await Set<TEntity>()
			.AddAsync(entity);

		await SaveChangesAsync();

		return entity;
	}

	public async Task<TEntity> Update(TEntity entity)
	{
		Set<TEntity>()
			.Update(entity);

		await SaveChangesAsync();

		return entity;
	}

	public async Task Delete(TEntity entity)
	{
		Set<TEntity>()
			.Remove(entity);

		await SaveChangesAsync();
	}

	public async Task<(bool Exists, TEntity? Entity)> CheckExistence(Guid id)
	{
		var entity = await GetById(id);

		return new(entity is not null, entity);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseInMemoryDatabase("TestsDb");
	}
}

internal class CategoriesRepository : InMemoryRepository<Category>, ICategoryRepository
{ }