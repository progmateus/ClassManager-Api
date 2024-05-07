using ClassManager.Domain.Contexts.Tenants.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Repositories.Contracts
{
  public interface ITenantPlanRepository : IRepository<TenantPlan>
  {
    Task<bool> PlanAlreadyExists(string name, CancellationToken cancellationToken);
    List<TenantPlan> ListByTenantId(Guid tenantId, CancellationToken cancellationToken);
    Task<TenantPlan> GetByIdAndTenantId(Guid tenantId, Guid planId, CancellationToken cancellationToken);
  }
}
