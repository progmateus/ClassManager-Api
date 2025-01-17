using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers.Tenants;

public class CreateTenantSubscriptionHandler : Notifiable,
  ITenantHandler<CreateSubscriptionCommand>
{
  private ISubscriptionRepository _subscriptionRepository;
  private ITenantRepository _tenantRepository;
  private IPlanRepository _planRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentService;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;

  public CreateTenantSubscriptionHandler(
    ISubscriptionRepository subscriptionRepository,
    ITenantRepository tenantRepository,
    IPlanRepository planRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IStripeCustomerRepository stripeCustomerRepository

  )
  {
    _subscriptionRepository = subscriptionRepository;
    _planRepository = planRepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
    _stripeCustomerRepository = stripeCustomerRepository;
    _tenantRepository = tenantRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateSubscriptionCommand command)
  {
    command.validateTenantSubscription();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    if (await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ACTIVE_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    var subscriptionsAlreadyExists = await _subscriptionRepository.GetSubscriptionsByStatus(null, tenantId, [ESubscriptionStatus.INCOMPLETE, ESubscriptionStatus.ACTIVE, ESubscriptionStatus.UNPAID, ESubscriptionStatus.PAST_DUE], ETargetType.TENANT);

    if (subscriptionsAlreadyExists.Any(x => x.Status == ESubscriptionStatus.ACTIVE || x.Status == ESubscriptionStatus.INCOMPLETE))
    {
      return new CommandResult(false, "ACTIVE_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    if (subscriptionsAlreadyExists.Any(x => x.Status != ESubscriptionStatus.ACTIVE && x.Status != ESubscriptionStatus.CANCELED && x.Status != ESubscriptionStatus.INCOMPLETE))
    {
      return new CommandResult(false, "UNPAID_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    var plan = await _planRepository.GetByIdAsync(command.PlanId, new CancellationToken());

    if (plan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    var stripeCustomer = await _stripeCustomerRepository.FindByUserIdAndTenantIdAndType(null, tenantId, ETargetType.TENANT);

    if (stripeCustomer is null)
    {
      return new CommandResult(false, "ERR_STRIPE_CUSTOMER_NOT_FOUND", null, null, 404);
    }

    var subscription = new Subscription(tenantId, plan.Id);

    var stripeSubscription = _paymentService.CreateSubscription(subscription.Id, subscription.UserId, tenantId, plan.StripePriceId, stripeCustomer.StripeCustomerId, ETargetType.TENANT, null);

    var stripeSubscriptionPriceItem = stripeSubscription.Items.Data.FirstOrDefault(x => x.Object == "subscription_item");

    if (stripeSubscriptionPriceItem is not null)
    {
      subscription.SetStripeSubscriptionPriceItemId(stripeSubscriptionPriceItem.Id);
    }

    subscription.SetStripeSubscriptionId(stripeSubscription.Id);
    subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());

    _paymentService.CreateInvoice(null, subscription.UserId, tenantId, stripeCustomer.StripeCustomerId, stripeSubscription.Id, null);

    return new CommandResult(true, "SUBSCRIPTION_CREATED", subscription, null, 201);
  }
}