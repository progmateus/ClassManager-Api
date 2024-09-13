using System.Linq.Expressions;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface ISubscriptionRepository : ITRepository<Subscription>
{
  Task<object> ListByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
  Task<Subscription> GetByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<Subscription>> ListSubscriptions(Guid? userId, Guid? tenantId);
  Task<bool> HasActiveSubscription(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<Subscription?> GetSubscriptionProfileAsync(Guid id, Guid tenantId, CancellationToken cancellationToken);
  Task<Subscription?> GetLatestSubscription(Guid tenantId, Guid userId, CancellationToken cancellationToken);
}