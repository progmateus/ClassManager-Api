using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class UpdateTenantInvoiceHandler : ITenantDeleteAction
{
  private ITenantInvoiceRepository _tenantInvoiceRepository;
  private readonly IAccessControlService _accessControlService;

  public UpdateTenantInvoiceHandler(
    ITenantInvoiceRepository tenantInvoiceRepository,
    IAccessControlService accessControlService
    )
  {
    _tenantInvoiceRepository = tenantInvoiceRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid invoiceId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var invoice = await _tenantInvoiceRepository.FindByIdAndTenantIdAsync(invoiceId, tenantId, default);

    if (invoice is null)
    {
      return new CommandResult(false, "ERR_INVOICE_NOT_FOUND", null, null, 404);
    }

    if (invoice.Status == EInvoiceStatus.PAYED)
    {
      return new CommandResult(false, "ERR_INVOICE_ALREADY_BEEN_PAYED", null, null, 409);
    }

    invoice.UpdateStatus(EInvoiceStatus.PAYED);

    return new CommandResult(true, "INVOICE_UPDATED", invoice, null, 201);
  }
}
