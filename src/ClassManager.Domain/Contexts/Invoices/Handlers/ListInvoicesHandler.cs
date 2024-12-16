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
    Guid? targetUserId = null;

    if (command.UserId.HasValue && command.UserId.Value != Guid.Empty)
    {
      if (await _accessControlService.CheckParameterUserIdPermission(command.TenantId, loggedUserId, command.UserId))
      {
        targetUserId = command.UserId.Value;
      }
      else
      {
        targetUserId = loggedUserId;
      }
    }

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var invoices = await _invoiceRepository.ListByUserIdAndTenantId(command.TenantId, targetUserId, command.SubscriptionId, command.Search, skip, command.Limit, new CancellationToken());

    return new CommandResult(true, "INVOICES_LISTED", invoices, null, 200);
  }
}
