using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;
using Microsoft.AspNetCore.Http;

namespace ClasManager.Domain.Contexts.Bookings.Handlers;

public class DeleteBookingHandler : Notifiable
{
  private IBookingRepository _bookingRepository;
  private IAccessControlService _accessControlService;
  public DeleteBookingHandler(
    IBookingRepository bookingRepository,
    IAccessControlService accessControlService
  )
  {
    _bookingRepository = bookingRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid bookingId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["student"]))
    {
      return new CommandResult(false, "ERR_USER_ROLE_NOT_FOUND", null, null, 403);
    }

    if (!await _accessControlService.IsUserActiveSubscriptionAsync(loggedUserId, tenantId))
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_ACTIVE", null, null, 403);
    }

    var booking = await _bookingRepository.FindAsync(x => x.Id == bookingId, [x => x.ClassDay]);

    if (booking is null)
    {
      return new CommandResult(false, "ERR_BOOKING_NOT_FOUND", null, null, 404);
    }

    if (booking.ClassDay.Status == EClassDayStatus.CONCLUDED)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_ALREADY_CONCLUDED", null, null, 403);
    }

    if (DateTime.Now > booking.ClassDay.Date)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_TIME_EXPIRED", null, null, 403);
    }

    await _bookingRepository.DeleteAsync(booking.Id, new CancellationToken());

    return new CommandResult(true, "BOOKING_DELETED", "", null, 201);
  }
}