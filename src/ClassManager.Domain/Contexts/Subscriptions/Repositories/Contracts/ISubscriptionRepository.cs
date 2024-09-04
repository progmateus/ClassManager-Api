using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface ISubscriptionRepository : IRepository<Subscription>
{
  Task<List<IGrouping<Guid, Subscription>>> ListByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
  Task<Subscription> GetByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<bool> HasActiveSubscription(Guid userId, Guid tenantId, CancellationToken cancellationToken);
}