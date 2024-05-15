using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace classManager.Data.Contexts.Subscriptions.Repositories;

public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
  public SubscriptionRepository(AppDbContext context) : base(context) { }

  public async Task<List<Subscription>> ListByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Where(x => x.TenantPlan.TenantId == tenantId).ToListAsync();
  }
}