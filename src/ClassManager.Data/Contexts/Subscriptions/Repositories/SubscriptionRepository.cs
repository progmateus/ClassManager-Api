using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace classManager.Data.Contexts.Subscriptions.Repositories;

public class SubscriptionRepository : TRepository<Subscription>, ISubscriptionRepository
{
  public SubscriptionRepository(AppDbContext context) : base(context) { }

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
    .ThenInclude(u => u.StudentsClasses.Where(sc => sc.Class.TenantId == tenantId))
    .ThenInclude(sc => sc.Class)
    .AsNoTracking()
    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
  }

  public async Task<Subscription?> FindUserLatestSubscription(Guid tenantId, Guid userId, CancellationToken cancellationToken)
  {
    return await DbSet
  .Include(x => x.TenantPlan)
  .AsNoTracking()
  .Where(x => x.UserId == userId && x.TenantId == tenantId)
  .OrderByDescending(x => x.CreatedAt)
  .FirstAsync();
  }

  public async Task<List<Subscription>> ListSubscriptions(List<Guid>? usersIds, List<Guid>? tenantsIds)
  {
    return await DbSet
    .Include(x => x.User)
    .Include(x => x.TenantPlan)
    .Include(x => x.Tenant)
    .Where(x => usersIds.Contains(x.UserId) || tenantsIds.Contains(x.TenantId))
    .GroupBy(x => new { x.TenantId, x.UserId })
    .Select(x => x.OrderByDescending(x => x.CreatedAt).Select(x => x).First()).ToListAsync();
  }

  public async Task<Subscription?> FindByStripeSubscriptionId(string stripeSubscriptionId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.StripeSubscriptionId == stripeSubscriptionId);
  }
}