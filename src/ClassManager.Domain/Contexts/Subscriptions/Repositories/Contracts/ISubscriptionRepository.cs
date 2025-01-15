using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface ISubscriptionRepository : ITRepository<Subscription>
{
  Task<Subscription> GetByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<Subscription>> ListSubscriptions(List<Guid>? usersIds, List<Guid>? tenantsIds, ETargetType targetType = ETargetType.USER, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
  Task<List<Subscription>> GetSubscriptionsByStatus(Guid? userId, Guid tenantId, List<ESubscriptionStatus> status, ETargetType targetType = ETargetType.USER);
  Task<Subscription?> GetSubscriptionProfileAsync(Guid id, Guid tenantId, CancellationToken cancellationToken);
  Task<Subscription?> FindLatestSubscription(Guid tenantId, Guid? userId, ETargetType? targetType = ETargetType.USER);
  Task<Subscription?> FindByStripeSubscriptionId(string stripeSubscriptionId, CancellationToken cancellationToken);
  Task<List<Subscription>> GetByTenantPlanIdAsync(Guid tenantPlanId, CancellationToken cancellationToken);
  Task<Subscription?> FindTenantLatestSubscriptionProfile(Guid tenantId, ETargetType? targetType = ETargetType.TENANT);
}