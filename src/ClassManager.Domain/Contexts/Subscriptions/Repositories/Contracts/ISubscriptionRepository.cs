using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface ISubscriptionRepository : ITRepository<Subscription>
{
  Task<Subscription> GetByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<Subscription>> ListSubscriptions(List<Guid>? usersIds, List<Guid>? tenantsIds, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
  Task<List<Subscription>> GetSubscriptionsByStatus(Guid userId, Guid tenantId, List<ESubscriptionStatus> status, CancellationToken cancellationToken);
  Task<Subscription?> GetSubscriptionProfileAsync(Guid id, Guid tenantId, CancellationToken cancellationToken);
  Task<Subscription?> FindUserLatestSubscription(Guid tenantId, Guid userId, CancellationToken cancellationToken);
  Task<Subscription?> FindByStripeSubscriptionId(string stripeSubscriptionId, CancellationToken cancellationToken);
}