using AutoMapper;
using ClasManager.Domain.Contexts.Invoices.Commands;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class CreateUserInvoiceHandler :
  Notifiable,
  ITenantHandler<CreateUserInvoiceCommand>
{
  private ISubscriptionRepository _subscriptionRepository;
  private IUserInvoiceRepository _userInvoiceRepository;
  private readonly IAccessControlService _accessControlService;

  public CreateUserInvoiceHandler(
    ISubscriptionRepository subscriptionRepository,
    IUserInvoiceRepository userInvoiceRepository,
    IAccessControlService accessControlService
    )
  {
    _subscriptionRepository = subscriptionRepository;
    _userInvoiceRepository = userInvoiceRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateUserInvoiceCommand command)
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

    if (await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var subscription = await _subscriptionRepository.FindAsync(x => x.Id == command.SubscriptionId && x.TenantId == tenantId, [x => x.TenantPlan]);

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (subscription.Status != Shared.Enums.ESubscriptionStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_ACTIVE", null, null, 404);
    }

    if (subscription.TenantPlan is null)
    {
      return new CommandResult(false, "ERR_TENANT_PLAN_NOT_FOUND", null, null, 404);
    }

    var userInvoice = new UserInvoice(subscription.UserId, subscription.TenantPlan.Id, subscription.Id, subscription.TenantPlan.Price);

    await _userInvoiceRepository.CreateAsync(userInvoice, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_CREATED", subscription, null, 201);
  }
}
