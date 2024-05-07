using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class GetTenantPlanByIdHandler
{
  private readonly ITenantPlanRepository _repository;
  public GetTenantPlanByIdHandler(
    ITenantPlanRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid planId)
  {
    var tenantPlans = await _repository.GetByIdAndTenantId(tenantId, planId, new CancellationToken());

    return new CommandResult(true, "PLAN_LISTED", tenantPlans, null, 200);
  }
}
