﻿using Application.Categories.Commands.CreateCategory;using Application.Categories.Queries.GetCategoriesWithPagination;using Application.Common.Exceptions;using Application.Repositories;using Application.Repositories.Common;using Domain.Entities;using Domain.Entities.Common;using Microsoft.EntityFrameworkCore;namespace Application.IntegrationTests;internal class InMemoryRepository<TEntity> : DbContext, IGenericRepository<TEntity>	where TEntity : BaseEntity{	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)	{		optionsBuilder.UseInMemoryDatabase("TestsDb");	}	public async Task<TEntity?> GetById(Guid id) => await Set<TEntity>().FindAsync(id);	public async Task<TEntity> Insert(TEntity entity)	{		await Set<TEntity>().AddAsync(entity);		await SaveChangesAsync();		return entity;	}	public async Task<TEntity> Update(TEntity entity)	{		Set<TEntity>().Update(entity);		await SaveChangesAsync();		return entity;	}	public DbSet<Category> Categories { get; set; }}internal class CategoriesRepository : InMemoryRepository<Category>, ICategoryRepository{ }