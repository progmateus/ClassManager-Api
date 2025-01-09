using ClasManager.Domain.Contexts.Bookings.Commands;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClasManager.Domain.Contexts.Bookings.Handlers;

public class ListBookingsHandler : IPaginationHandler<ListBookingsCommand>
{
  private IBookingRepository _bookingRepository;
  private IAccessControlService _accessControlService;
  public ListBookingsHandler(
    IBookingRepository bookingRepository,
    IAccessControlService accessControlService
    )
  {
    _bookingRepository = bookingRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, ListBookingsCommand command)
  {

    var targetUserId = loggedUserId;

    if (command.UserId.HasValue && command.UserId != Guid.Empty)
    {
      if (await _accessControlService.CheckParameterUserIdPermission(command.TenantId, loggedUserId, command.UserId))
      {
        targetUserId = command.UserId.Value;
      }
      else
      {
        return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, 404);
      }
    }

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var bookings = await _bookingRepository.ListByUserIdOrTenantIdOrClassDayIdWithPagination(command.TenantId, targetUserId, command.ClassDayId, command.Search, skip, command.Limit, new CancellationToken());

    return new CommandResult(true, "BOOKINGS_LISTED", bookings, null, 200);
  }
}