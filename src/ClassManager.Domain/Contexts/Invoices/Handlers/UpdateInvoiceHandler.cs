using ClasManager.Domain.Contexts.Invoices.Commands;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class UpdateInvoiceHandler :
  Notifiable,
  ITenantActionHandler<UpdateInvoiceCommand>
{
  private IInvoiceRepository _invoiceRepository;
  private readonly IAccessControlService _accessControlService;

  public UpdateInvoiceHandler(
    IInvoiceRepository invoiceRepository,
    IAccessControlService accessControlService
    )
  {
    _invoiceRepository = invoiceRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid invoiceId, UpdateInvoiceCommand command)
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

    var invoice = await _invoiceRepository.FindByIdAndTenantIdAsync(invoiceId, tenantId, default);

    if (invoice is null)
    {
      return new CommandResult(false, "ERR_INVOICE_NOT_FOUND", null, null, 404);
    }

    if (invoice is null || invoice.TargetType != EInvoiceTargetType.USER)
    {
      return new CommandResult(false, "ERR_CANNOT_UPDATE_TENANT_INVOICE", null, null, 404);
    }

    if (invoice.Status == EInvoiceStatus.PAYED)
    {
      return new CommandResult(false, "ERR_INVOICE_ALREADY_BEEN_PAYED", null, null, 409);
    }


    invoice.UpdateStatus(EInvoiceStatus.PAYED);

    await _invoiceRepository.UpdateAsync(invoice, new CancellationToken());

    return new CommandResult(true, "INVOICE_UPDATED", invoice, null, 201);
  }
}
