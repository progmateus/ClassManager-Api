using ClassManager.Domain.Contexts.Tenants.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Repositories.Contracts
{
  public interface ITenantPlanRepository : IRepository<TenantPlan>
  {
    Task<bool> PlanAlreadyExists(string name, CancellationToken cancellationToken);
    Task<List<TenantPlan>> ListByTenantId(Guid tenantId, CancellationToken cancellationToken);
    Task<TenantPlan> GetByIdAndTenantId(Guid tenantId, Guid tenantPlanId, CancellationToken cancellationToken);
    Task<TenantPlan?> FindByStripePriceId(string stripePriceId, CancellationToken cancellationToken = default);
  }
}
