using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface IStripeCustomerRepository : ITRepository<StripeCustomer>
{
  Task<StripeCustomer?> FindByUserIdAndTenantIdAndType(Guid? userId, Guid tenantId, EStripeCustomerType type, CancellationToken cancellationToken = default);
  Task<StripeCustomer?> FindByCustomerId(string customerId, CancellationToken cancellationToken);
}