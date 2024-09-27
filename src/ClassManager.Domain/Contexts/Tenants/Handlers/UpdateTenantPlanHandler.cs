using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class UpdateTenantPlanHandler :
  Notifiable,
  ITenantActionHandler<TenantPlanCommand>
{
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly IAccessControlService _accessControlService;


  public UpdateTenantPlanHandler(
    ITenantPlanRepository tenantPlanRepository,
    IAccessControlService accessControlService

    )
  {
    _tenantPlanRepository = tenantPlanRepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid planId, TenantPlanCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
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
