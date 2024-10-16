using System.Linq.Expressions;
using ClassManager.Domain.Shared.Entities;

public interface ITRepository<TEntity> where TEntity : TenantEntity
{
  Task<TEntity?> FindByIdAndTenantIdAsync(Guid id, Guid tenantId, CancellationToken cancellationToken);
  Task<List<TEntity>> ListByTenantId(Guid tenantId, CancellationToken cancellationToken);
  Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
  Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
  Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
  Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  Task DeleteAsync(Guid id, Guid tenantId, CancellationToken cancellationToken);
  Task<bool> IdExistsAsync(Guid id, Guid tenantId, CancellationToken cancellationToken);
  Task<List<TEntity>> GetByIdsAsync(List<Guid> ids, Guid tenantId, CancellationToken cancellationToken);
  Task CreateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
  Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
  Task DeleteAllByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
}