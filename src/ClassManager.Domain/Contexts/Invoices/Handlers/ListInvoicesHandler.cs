using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
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
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid? tenantId, Guid? userId)
  {
    if (tenantId != Guid.Empty && tenantId != Guid.Empty && !await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId.Value))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var userTargetId = loggedUserId;

    if (userId.HasValue && userId != Guid.Empty)
    {
      if (tenantId == Guid.Empty)
      {
        return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
      }

      if (userId.HasValue && userId != Guid.Empty && userId.Value != loggedUserId && !await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId.Value, ["admin"]))
      {
        return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
      }

      userTargetId = userId.Value;
    }

    var invoices = await _invoiceRepository.ListByUserIdAndTenantId(tenantId, userTargetId);

    return new CommandResult(true, "INVOICES_LISTED", invoices, null, 200);
  }
}
