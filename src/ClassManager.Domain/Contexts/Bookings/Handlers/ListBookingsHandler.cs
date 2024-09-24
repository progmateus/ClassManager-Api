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
  private IUserRepository _userRepository;
  private IUsersRolesRepository _usersRolesRepository;
  private ISubscriptionRepository _subscriptionRepository;
  private IAccessControlService _accessControlService;
  public ListBookingsHandler(
    IBookingRepository bookingRepository,
    IUserRepository userRepository,
    IUsersRolesRepository usersRolesRepository,
    ISubscriptionRepository subscriptionRepository,
    IAccessControlService accessControlService
    )
  {
    _bookingRepository = bookingRepository;
    _userRepository = userRepository;
    _usersRolesRepository = usersRolesRepository;
    _subscriptionRepository = subscriptionRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid? tenantId, Guid userId)
  {
    if (tenantId.HasValue && tenantId != Guid.Empty)
    {
      if (!await _accessControlService.HasUserRoleAsync(userId, tenantId.Value, "admin"))
      {
        return new CommandResult(false, "ERR_STUDENT_ROLE_NOT_FOUND", null, 404);
      }

      var subscription = await _subscriptionRepository.GetByUserIdAndTenantId(userId, tenantId.Value, new CancellationToken());

      if (subscription is null)
      {
        return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
      }
    }

    var bookings = await _bookingRepository.ListByUserIdAndTenantId(tenantId, userId);

    return new CommandResult(true, "BOOKINGS_LISTED", bookings, null, 200);
  }
}