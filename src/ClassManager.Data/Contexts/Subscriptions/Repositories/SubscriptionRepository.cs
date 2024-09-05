using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace classManager.Data.Contexts.Subscriptions.Repositories;

public class SubscriptionRepository : TRepository<Subscription>, ISubscriptionRepository
{
  public SubscriptionRepository(AppDbContext context) : base(context) { }

  public async Task<object> ListByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)
  {

    return await DbSet
    .Include(x => x.User)
    .Include(x => x.TenantPlan)
    .Where(x => x.TenantId == tenantId)
    .GroupBy(x => x.UserId)
    .Select(x => new
    {
      Subscription = x.OrderByDescending(x => x.CreatedAt).Select(x => x).First()
    }).ToListAsync(cancellationToken);
  }

  public async Task<Subscription> GetByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Include(x => x.TenantPlan).FirstAsync(x => x.UserId == userId && x.TenantPlan.TenantId == tenantId);
  }

  public async Task<bool> HasActiveSubscription(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Include(x => x.TenantPlan).AsNoTracking().AnyAsync(x => x.TenantPlan.TenantId == tenantId && x.UserId == userId && x.Status == ESubscriptionStatus.ACTIVE, cancellationToken);
  }

  public async Task<Subscription?> GetSubscriptionProfileAsync(Guid id, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet
    .Include(x => x.TenantPlan)
    .Include(x => x.Tenant)
    .Include(x => x.User)
    .ThenInclude(u => u.Classes.Where(x => x.TenantId == tenantId))
    .AsNoTracking()
    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
  }
}