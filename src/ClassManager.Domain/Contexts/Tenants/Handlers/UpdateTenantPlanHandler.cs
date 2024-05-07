using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class UpdateTenantPlanHandler :
  Notifiable,
  ITenantActionHandler<TenantPlanCommand>
{
  private readonly ITenantPlanRepository _tenantPlanRepository;

  public UpdateTenantPlanHandler(
    ITenantPlanRepository tenantPlanRepository
    )
  {
    _tenantPlanRepository = tenantPlanRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid planId, TenantPlanCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_PLAN_NOT_UPDATED", null, command.Notifications);
    }
    var tenantPlan = await _tenantPlanRepository.GetByIdAndTenantId(tenantId, planId, new CancellationToken());

    if (tenantPlan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }
    tenantPlan.ChangeTenantPlan(command.Name, command.Description, command.TimesOfWeek, command.Price);
    await _tenantPlanRepository.UpdateAsync(tenantPlan, new CancellationToken());

    return new CommandResult(true, "PLAN_UPDATED", tenantPlan, null, 200);
  }
}
