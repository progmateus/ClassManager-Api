using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace classManager.Data.Contexts.Subscriptions.Repositories;

public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
  public SubscriptionRepository(AppDbContext context) : base(context) { }

  public async Task<object> ListByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)
  {

    return await DbSet
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

}