using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class DeleteInvoiceHandler : ITenantDeleteAction
{
  private IInvoiceRepository _invoiceRepository;
  private readonly IAccessControlService _accessControlService;

  public DeleteInvoiceHandler(
    IInvoiceRepository invoiceRepository,
    IAccessControlService accessControlService
    )
  {
    _invoiceRepository = invoiceRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid invoiceId)
  {


    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
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

    if (invoice is null || invoice.TargetType != ETargetType.USER)
    {
      return new CommandResult(false, "ERR_CANNOT_UPDATE_TENANT_INVOICE", null, null, 404);
    }

    if (invoice.Status == EInvoiceStatus.PAID)
    {
      return new CommandResult(false, "ERR_INVOICE_ALREADY_BEEN_PAID", null, null, 409);
    }

    await _invoiceRepository.DeleteAsync(invoice.Id, tenantId, new CancellationToken());

    return new CommandResult(true, "INVOICE_UPDATED", invoice, null, 201);
  }
}
