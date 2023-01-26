using Application.Repositories;
using Application.Repositories.Common;
using Domain.Entities;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests;

internal class InMemoryRepository<TEntity> : DbContext, IGenericRepository<TEntity>
	where TEntity : BaseEntity
{
	public DbSet<Category> Categories { get; set; }

	public async Task<TEntity?> GetById(Guid id) => await Set<TEntity>().FindAsync(id);

	public async Task<TEntity> Insert(TEntity entity)
	{
		await Set<TEntity>().AddAsync(entity);

		await SaveChangesAsync();

		return entity;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseInMemoryDatabase("TestsDb");
	}
}

internal class CategoriesRepository : InMemoryRepository<Category>, ICategoryRepository
{ }