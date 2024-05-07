using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Repositories.Contracts
{
  public interface ITenantPlanRepository : IRepository<TenantPlan>
  {
    Task<bool> PlanAlreadyExists(string name, CancellationToken cancellationToken);
    List<TenantPlan> ListByTenantId(Guid tenantId, CancellationToken cancellationToken);
    Task<TenantPlan> GetByIdAndTenantId(Guid planId, Guid tenantId, CancellationToken cancellationToken);
  }
}
