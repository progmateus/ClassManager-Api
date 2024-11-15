using System.Linq.Expressions;
using ClassManager.Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.shared.Repositories;

public abstract class TRepository<TEntity> : ITRepository<TEntity> where TEntity : TenantEntity
{

  protected readonly DbContext DbContext;
  protected readonly DbSet<TEntity> DbSet;

  protected TRepository(DbContext dbContext)
  {
    DbContext = dbContext;
    DbSet = DbContext.Set<TEntity>();
  }
  public async virtual Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
  {
    DbSet.Add(entity);
    await SaveChangesAsync(cancellationToken);
  }

  public async Task<TEntity?> FindByIdAndTenantIdAsync(Guid id, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId, cancellationToken);
  }

  public async virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
  {
    DbSet.Update(entity);
    await SaveChangesAsync(cancellationToken);
  }

  public virtual async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken cancellationToken)
  {
    var user = await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId, cancellationToken);
    if (user != null)
    {
      DbSet.Remove(user);
      await SaveChangesAsync(cancellationToken);
    }
  }

  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
  {
    return await DbContext.SaveChangesAsync(cancellationToken);
  }

  public async Task<bool> IdExistsAsync(Guid id, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().AnyAsync(x => x.Id == id && x.TenantId == tenantId, cancellationToken);
  }

  public async Task<List<TEntity>> GetByIdsAsync(List<Guid> ids, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Where(x => ids.Contains(x.Id) && x.TenantId == tenantId).ToListAsync(cancellationToken);
  }

  public async Task CreateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
  {
    DbSet.AddRange(entities);
    await SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(entities);
    await SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAllByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where(x => x.TenantId == tenantId));
    await SaveChangesAsync(cancellationToken);
  }

  public async Task<List<TEntity>> ListByTenantId(Guid tenantId, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet.Where(x => x.TenantId == tenantId).Skip(skip).Take(limit).ToListAsync(cancellationToken);
  }

  public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
  {
    var query = DbSet.AsNoTracking().Where(predicate);

    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
  }

  public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
  {
    var query = DbSet.AsNoTracking().Where(predicate);

    return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).FirstOrDefaultAsync();
  }

  public async Task<List<TEntity>> ListByTenantIdWithPagination(Guid tenantId, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet.Where(x => x.TenantId == tenantId).Skip(skip).Take(limit).ToListAsync(cancellationToken);
  }
}