using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface IStripeCustomerRepository : ITRepository<StripeCustomer>
{
  Task<StripeCustomer?> FindByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
}