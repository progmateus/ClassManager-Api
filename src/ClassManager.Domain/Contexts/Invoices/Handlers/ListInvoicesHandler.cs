using ClasManager.Domain.Contexts.Invoices.Commands;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class ListInvoicesHandler : IPaginationHandler<ListInvoicesCommand>
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
  public async Task<ICommandResult> Handle(Guid loggedUserId, ListInvoicesCommand command)
  {
    if (command.TenantId.HasValue && command.TenantId != Guid.Empty && command.TenantId != Guid.Empty && !await _accessControlService.IsTenantSubscriptionActiveAsync(command.TenantId.Value))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var userTargetId = loggedUserId;

    if (command.UserId.HasValue && command.UserId != Guid.Empty)
    {
      if (command.TenantId == Guid.Empty)
      {
        return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
      }

      if (command.UserId.HasValue && command.UserId != Guid.Empty && command.UserId.Value != loggedUserId && !await _accessControlService.HasUserAnyRoleAsync(loggedUserId, command.TenantId.Value, ["admin"]))
      {
        return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
      }

      userTargetId = command.UserId.Value;
    }

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var invoices = await _invoiceRepository.ListByUserIdAndTenantId(command.TenantId, userTargetId, command.Search, skip, command.Limit, new CancellationToken());

    return new CommandResult(true, "INVOICES_LISTED", invoices, null, 200);
  }
}
