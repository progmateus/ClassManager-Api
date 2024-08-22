using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class ListTenantPlansHandler
{
  private readonly ITenantPlanRepository _repository;
  public ListTenantPlansHandler(
    ITenantPlanRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId)
  {
    var tenantPlans = await _repository.ListByTenantId(tenantId, new CancellationToken());

    return new CommandResult(true, "PLANS_LISTED", tenantPlans, null, 200);
  }
}
