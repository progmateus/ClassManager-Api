using ClasManager.Domain.Contexts.Bookings.Commands;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClasManager.Domain.Contexts.Bookings.Handlers;

public class ListBookingsHandler : IPaginationHandler<ListSubscriptionsCommand>
{
  private IBookingRepository _bookingRepository;
  private ISubscriptionRepository _subscriptionRepository;
  private IAccessControlService _accessControlService;
  public ListBookingsHandler(
    IBookingRepository bookingRepository,
    ISubscriptionRepository subscriptionRepository,
    IAccessControlService accessControlService
    )
  {
    _bookingRepository = bookingRepository;
    _subscriptionRepository = subscriptionRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, ListSubscriptionsCommand command)
  {

    var userIdNotEmpty = loggedUserId;

    if (command.UserId.HasValue && command.UserId != Guid.Empty)
    {
      if (command.UserId.Value != loggedUserId)
      {
        if (!command.TenantId.HasValue || command.TenantId == Guid.Empty)
        {
          return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, 404);
        }

        if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, command.TenantId.Value, ["admin"]))
        {
          return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, 404);
        }
        userIdNotEmpty = command.UserId.Value;
      }
    }

    if (command.TenantId.HasValue && command.TenantId != Guid.Empty)
    {
      var subscription = await _subscriptionRepository.GetByUserIdAndTenantId(userIdNotEmpty, command.TenantId.Value, new CancellationToken());

      if (subscription is null)
      {
        return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
      }
    }

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var bookings = await _bookingRepository.ListByUserIdAndTenantIdWithPagination(command.TenantId, userIdNotEmpty, command.Search, skip, command.Limit, new CancellationToken());

    return new CommandResult(true, "BOOKINGS_LISTED", bookings, null, 200);
  }
}