using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClasManager.Domain.Contexts.Bookings.Handlers;

public class ListBookingsHandler : Notifiable
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
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid? tenantId, Guid? userId)
  {

    var userIdNotEmpty = loggedUserId;

    if (userId.HasValue && userId != Guid.Empty)
    {
      if (userId.Value != loggedUserId)
      {
        if (!tenantId.HasValue || tenantId == Guid.Empty)
        {
          return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, 404);
        }

        if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId.Value, ["admin"]))
        {
          return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, 404);
        }
        userIdNotEmpty = userId.Value;
      }
    }

    if (tenantId.HasValue && tenantId != Guid.Empty)
    {
      var subscription = await _subscriptionRepository.GetByUserIdAndTenantId(userIdNotEmpty, tenantId.Value, new CancellationToken());

      if (subscription is null)
      {
        return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
      }
    }

    var bookings = await _bookingRepository.ListByUserIdAndTenantId(tenantId, userIdNotEmpty);

    return new CommandResult(true, "BOOKINGS_LISTED", bookings, null, 200);
  }
}