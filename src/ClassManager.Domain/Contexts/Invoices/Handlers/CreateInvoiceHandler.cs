using ClasManager.Domain.Contexts.Invoices.Commands;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class CreateInvoiceHandler :
  Notifiable,
  ITenantHandler<CreateInvoiceCommand>
{
  private ISubscriptionRepository _subscriptionRepository;
  private IInvoiceRepository _invoiceRepository;
  private readonly IAccessControlService _accessControlService;

  public CreateInvoiceHandler(
    ISubscriptionRepository subscriptionRepository,
    IInvoiceRepository invoiceRepository,
    IAccessControlService accessControlService
    )
  {
    _subscriptionRepository = subscriptionRepository;
    _invoiceRepository = invoiceRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateInvoiceCommand command)
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

    if (subscription.Status != ESubscriptionStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_ACTIVE", null, null, 404);
    }

    if (subscription.TenantPlan is null)
    {
      return new CommandResult(false, "ERR_TENANT_PLAN_NOT_FOUND", null, null, 404);
    }

    var invoice = new Invoice(subscription.UserId, subscription.TenantPlan.Id, subscription.Id, null, subscription.TenantPlan.TenantId, EInvoiceTargetType.USER, EInvoiceType.USER_SUBSCRIPTION);
    invoice.SetExpiresDate();

    await _invoiceRepository.CreateAsync(invoice, new CancellationToken());

    return new CommandResult(true, "INVOICE_CREATED", subscription, null, 201);
  }
}
