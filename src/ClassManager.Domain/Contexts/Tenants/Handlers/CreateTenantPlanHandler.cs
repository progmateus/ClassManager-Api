using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class CreateTenantPlanHandler :
  Notifiable,
  ITenantHandler<TenantPlanCommand>
{
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IStripeService _stripeService;

  public CreateTenantPlanHandler(
    ITenantPlanRepository tenantPlanRepository,
    IAccessControlService accessControlService,
    IStripeService stripeService

    )
  {
    _tenantPlanRepository = tenantPlanRepository;
    _accessControlService = accessControlService;
    _stripeService = stripeService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, TenantPlanCommand command)
  {
    // fail fast validation
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

    if (await _tenantPlanRepository.PlanAlreadyExists(command.Name, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_PLAN_ALREADY_EXISTS", null, null, 409);
    }

    var tenantPlan = new TenantPlan(command.Name, command.Description, command.TimesOfWeek, tenantId, command.Price);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, null, 400);
    }

    var stripeProduct = _stripeService.CreateProduct(tenantPlan.Id, "tenant", tenantPlan.Name, tenantId);

    _stripeService.CreatePrice(tenantPlan.Id, tenantId, stripeProduct.Id, tenantPlan.Price * 100);

    tenantPlan.SetStripeProductId(stripeProduct.Id);

    await _tenantPlanRepository.CreateAsync(tenantPlan, new CancellationToken());

    return new CommandResult(true, "PLAN_CREATED", tenantPlan, null, 201);
  }
}
