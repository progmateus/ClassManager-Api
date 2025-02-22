using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Libs.MassTransit.Events;
using ClassManager.Domain.Libs.MassTransit.Publish;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
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
  private readonly IPaymentService _paymentservice;
  private IPublishBus _publishBus;



  public UpdateTenantPlanHandler(
    ITenantPlanRepository tenantPlanRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IPublishBus publishBus

    )
  {
    _tenantPlanRepository = tenantPlanRepository;
    _accessControlService = accessControlService;
    _paymentservice = paymentService;
    _publishBus = publishBus;

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
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var tenantPlan = await _tenantPlanRepository.FindAsync(x => x.Id == planId && x.TenantId == tenantId, [x => x.Tenant]);

    if (tenantPlan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    var oldTenantPlanPrice = tenantPlan.Price;
    var stripePriceId = tenantPlan.StripePriceId;

    if (command.Price != tenantPlan.Price)
    {
      var stripePrice = _paymentservice.CreatePrice(tenantPlan.Id, tenantId, tenantPlan.StripeProductId, command.Price * 100, tenantPlan.Tenant.StripeAccountId);
      stripePriceId = stripePrice.Id;
    }

    _paymentservice.UpdateProduct(tenantPlan.StripeProductId, stripePriceId, command.Name, command.Description, tenantPlan.Tenant.StripeAccountId);

    tenantPlan.SetStripeInformations(stripePriceId, tenantPlan.StripeProductId);
    tenantPlan.ChangeTenantPlan(command.Name, command.Description, command.TimesOfWeek, command.Price);
    await _tenantPlanRepository.UpdateAsync(tenantPlan, new CancellationToken());

    if (oldTenantPlanPrice != command.Price)
    {
      var eventRequest = new UpdatesubscriptionsPricesEvent(tenantPlan.Id);
      await _publishBus.PublicAsync(eventRequest);
    }

    return new CommandResult(true, "PLAN_UPDATED", tenantPlan, null, 200);
  }
}
