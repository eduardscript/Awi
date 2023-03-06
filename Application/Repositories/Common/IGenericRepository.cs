using Domain.Entities.Common;

namespace Application.Repositories.Common;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
	Task<TEntity?> GetById(Guid id);

	Task<TEntity> Insert(TEntity entity);

	Task<TEntity> Update(TEntity entity);

	Task Delete(TEntity entity);
	
	Task<(bool Exists, TEntity? Entity)> CheckExistence(Guid id);
}