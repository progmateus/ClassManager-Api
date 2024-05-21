using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using ClassManager.Domain.Shared.Entities;

public interface IRepository<TEntity> where TEntity : Entity
{
  Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
  Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
  Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
  Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
  Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  Task DeleteAsync(Guid id, CancellationToken cancellationToken);
  Task<bool> IdExistsAsync(Guid id, CancellationToken cancellationToken);
  Task<List<TEntity>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
  Task CreateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
  Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
}