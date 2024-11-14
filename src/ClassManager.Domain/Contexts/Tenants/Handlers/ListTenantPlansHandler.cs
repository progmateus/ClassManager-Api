using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class ListTenantPlansHandler : ITenantPaginationHandler<PaginationCommand>
{
  private readonly ITenantPlanRepository _repository;
  public ListTenantPlansHandler(
    ITenantPlanRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, PaginationCommand command)
  {

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var tenantPlans = await _repository.ListByTenantIdWithPagination(tenantId, command.Search, skip, command.Limit, new CancellationToken());

    return new CommandResult(true, "PLANS_LISTED", tenantPlans, null, 200);
  }
}
