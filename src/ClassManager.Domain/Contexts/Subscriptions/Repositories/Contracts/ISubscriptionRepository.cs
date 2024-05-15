using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface ISubscriptionRepository : IRepository<Subscription>
{
  Task<List<Subscription>> ListByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken);
}