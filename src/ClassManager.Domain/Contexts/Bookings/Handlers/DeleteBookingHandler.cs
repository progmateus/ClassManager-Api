using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;
using Microsoft.AspNetCore.Http;

namespace ClasManager.Domain.Contexts.Bookings.Handlers;

public class DeleteBookingHandler : Notifiable
{
  private ITenantRepository _tenantRepository;
  private IBookingRepository _bookingRepository;
  private IClassDayRepository _classDayRepository;
  private IUserRepository _userRepository;
  private IUsersRolesRepository _usersRolesRepository;
  private ISubscriptionRepository _subscriptionRepository;
  public DeleteBookingHandler(ITenantRepository tenantRepository, IBookingRepository bookingRepository, IClassDayRepository classDayRepository, IUserRepository userRepository, IUsersRolesRepository usersRolesRepository, ISubscriptionRepository subscriptionRepository)
  {
    _tenantRepository = tenantRepository;
    _bookingRepository = bookingRepository;
    _classDayRepository = classDayRepository;
    _userRepository = userRepository;
    _usersRolesRepository = usersRolesRepository;
    _subscriptionRepository = subscriptionRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid bookingId, Guid userId)
  {

    var tenant = await _tenantRepository.GetByIdAndIncludePlanAsync(tenantId, new CancellationToken());

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_DAY_NOT_FOUND", null, 404);
    }

    if (tenant.Status != ETenantStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var user = await _userRepository.IdExistsAsync(userId, new CancellationToken());

    if (!user)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null);
    }

    var userRole = await _usersRolesRepository.VerifyRoleExistsAsync(userId, tenantId, "student", new CancellationToken());

    if (!userRole)
    {
      return new CommandResult(false, "ERR_STUDENT_ROLE_NOT_FOUND", null, 404);
    }

    var subscription = await _subscriptionRepository.GetByUserIdAndTenantId(userId, tenantId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (subscription.Status != ESubscriptionStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_ACTIVE", null, null, 403);
    }

    var booking = await _bookingRepository.GetWithInclude(userId, bookingId);

    if (booking is null)
    {
      return new CommandResult(false, "ERR_BOOKING_NOT_FOUND", null, null, 404);
    }

    if (booking.ClassDay.Status == EClassDayStatus.CONCLUDED)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_ALREADY_CONCLUDED", null, null, 403);
    }

    await _bookingRepository.DeleteAsync(booking.Id, new CancellationToken());

    return new CommandResult(true, "BOOKING_DELETED", "", null, 201);
  }
}