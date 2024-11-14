using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class ListInvoicesHandler
{
  private IInvoiceRepository _invoiceRepository;
  private readonly IAccessControlService _accessControlService;

  public ListInvoicesHandler(
    IInvoiceRepository invoiceRepository,
    IAccessControlService accessControlService
    )
  {
    _invoiceRepository = invoiceRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid? tenantId, Guid userId)
  {
    if (tenantId.HasValue && !await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId.Value))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (tenantId.HasValue && await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId.Value, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    return new CommandResult(true, "INVOICE_UPDATED", "", null, 201);
  }
}
