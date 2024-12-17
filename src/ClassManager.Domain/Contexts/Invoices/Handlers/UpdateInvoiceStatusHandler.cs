using ClasManager.Domain.Contexts.Invoices.Commands;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class UpdateInvoiceStatusHandler :
  Notifiable,
  ITenantActionHandler<UpdateInvoiceCommand>
{
  private IInvoiceRepository _invoiceRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentService;

  public UpdateInvoiceStatusHandler(
    IInvoiceRepository invoiceRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService
    )
  {
    _invoiceRepository = invoiceRepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid invoiceId, UpdateInvoiceCommand command)
  {
    if (command.Status != EInvoiceStatus.PAID)
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var invoice = await _invoiceRepository.FindByIdAndTenantIdAsync(invoiceId, tenantId, default);

    if (invoice is null)
    {
      return new CommandResult(false, "ERR_INVOICE_NOT_FOUND", null, null, 404);
    }

    if (invoice.TargetType != EInvoiceTargetType.USER || invoice.Type != EInvoiceType.USER_SUBSCRIPTION)
    {
      return new CommandResult(false, "ERR_CANNOT_UPDATE_TENANT_INVOICE", null, null, 404);
    }

    if (invoice.Status == EInvoiceStatus.PAID)
    {
      return new CommandResult(false, "ERR_INVOICE_ALREADY_BEEN_PAID", null, null, 409);
    }

    _paymentService.PayInvoice(invoice.StripeInvoiceId, null);

    return new CommandResult(true, "INVOICE_UPDATED", new { }, null, 201);
  }
}
