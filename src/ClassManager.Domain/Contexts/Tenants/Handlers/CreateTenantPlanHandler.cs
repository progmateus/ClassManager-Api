using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class CreateTenantPlanHandler :
  Notifiable,
  IActionHandler<TenantPlanCommand>
{
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly ITenantRepository _tenantRepository;

  public CreateTenantPlanHandler(
    ITenantPlanRepository tenantPlanRepository,
    ITenantRepository tenantRepository
    )
  {
    _tenantPlanRepository = tenantPlanRepository;
    _tenantRepository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, TenantPlanCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_PLAN_NOT_CREATED", null, command.Notifications);
    }
    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());
    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    if (await _tenantPlanRepository.PlanAlreadyExists(command.Name, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_PLAN_ALREADY_EXISTS", null, null, 409);
    }

    var tenantPlan = new TenantPlan(command.Name, command.Description, command.TimesOfWeek, tenantId);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_CREATED", null, null, 400);
    }


    await _tenantPlanRepository.CreateAsync(tenantPlan, new CancellationToken());

    return new CommandResult(true, "PLAN_CREATED", tenantPlan, null, 201);
  }
}
