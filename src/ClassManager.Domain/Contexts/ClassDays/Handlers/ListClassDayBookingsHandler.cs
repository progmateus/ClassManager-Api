using ClasManager.Domain.Contexts.Bookings.Commands;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClasManager.Domain.Contexts.ClassDays.Handlers;

public class ListClassDayBookingsHandler : ITenantPaginationHandler<ListBookingsCommand>
{
  private IBookingRepository _bookingRepository;
  public ListClassDayBookingsHandler(
    IBookingRepository bookingRepository
    )
  {
    _bookingRepository = bookingRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, ListBookingsCommand command)
  {

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var bookings = await _bookingRepository.ListByUserIdOrTenantIdOrClassDayIdWithPagination(tenantId, null, command.ClassDayId, command.Search, skip, command.Limit, new CancellationToken());

    return new CommandResult(true, "BOOKINGS_LISTED", bookings, null, 200);
  }
}