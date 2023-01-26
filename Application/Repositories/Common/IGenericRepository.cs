using Domain.Entities.Common;

namespace Application.Repositories.Common;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
	Task<TEntity?> GetById(Guid id);

	Task<TEntity> Insert(TEntity entity);
}