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

  public async Task<List<Subscription>> GetSubscriptionsByStatus(Guid? userId, Guid tenantId, List<ESubscriptionStatus> status, ETargetType targetType = ETargetType.USER)
  {
    return await DbSet
    .Include(x => x.TenantPlan)
    .AsNoTracking()
    .Where(x => x.TenantId == tenantId)
    .Where(x => status.Contains(x.Status))
    .Where(x => !userId.HasValue || x.UserId == userId)
    .Where(x => x.TargetType == targetType)
    .ToListAsync();
  }

  public async Task<Subscription?> GetSubscriptionProfileAsync(Guid id, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet
    .Include(x => x.TenantPlan)
    .Include(x => x.Tenant)
    .Include(x => x.LatestInvoice)
    .Include(x => x.User)
    .ThenInclude(x => x.Address)
    .Include(x => x.User)
    .ThenInclude(u => u.StudentsClasses.Where(sc => sc.Class.TenantId == tenantId))
    .ThenInclude(sc => sc.Class)
    .AsNoTracking()
    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
  }

  public async Task<Subscription?> FindLatestSubscription(Guid tenantId, Guid? userId, ETargetType? targetType = ETargetType.USER)
  {
    return await DbSet
  .Include(x => x.TenantPlan)
  .AsNoTracking()
  .OrderByDescending(x => x.CreatedAt)
  .FirstOrDefaultAsync(x => (!userId.HasValue || x.UserId == userId) && x.TenantId == tenantId && x.TargetType == targetType);
  }

  public async Task<List<Subscription>> ListSubscriptions(List<Guid>? usersIds, List<Guid>? tenantsIds, ETargetType targetType = ETargetType.USER, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet
    .Include(x => x.User)
    .Include(x => x.TenantPlan)
    .Include(x => x.Tenant)
    .Include(x => x.Invoices)
    .Where(x => usersIds.IsNullOrEmpty() || usersIds.Contains(x.UserId.Value))
    .Where(x => tenantsIds.IsNullOrEmpty() || tenantsIds.Contains(x.TenantId))
    .Where(x => search.IsNullOrEmpty() || x.User.Username.Contains(search) || x.User.Name.Contains(search))
    .Where(x => x.TargetType == targetType)
    .GroupBy(x => new { x.TenantId, x.UserId })
    .Select(x => x.OrderByDescending(x => x.CreatedAt).Select(x => x).First())
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }

  public async Task<Subscription?> FindByStripeSubscriptionId(string stripeSubscriptionId, CancellationToken cancellationToken)
  {
    return await DbSet.Include(x => x.TenantPlan).Include(x => x.Plan).Include(x => x.LatestInvoice).FirstOrDefaultAsync(x => x.StripeSubscriptionId == stripeSubscriptionId);
  }

  public async Task<List<Subscription>> GetByTenantPlanIdAsync(Guid tenantPlanId, CancellationToken cancellationToken)
  {
    return await DbSet
    .Where(x => x.TenantPlanId == tenantPlanId)
    .Where(x => x.Status != ESubscriptionStatus.INCOMPLETE_EXPIRED && x.Status != ESubscriptionStatus.CANCELED)
    .ToListAsync(cancellationToken);
  }
}