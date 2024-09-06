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

public class CreateBookingHandler : Notifiable, ITenantHandler<CreateBookingCommand>
{
  private ITenantRepository _tenantRepository;
  private IBookingRepository _bookingRepository;
  private IClassDayRepository _classDayRepository;
  private IUserRepository _userRepository;
  private IUsersRolesRepository _usersRolesRepository;
  private ISubscriptionRepository _subscriptionRepository;
  private IStudentsClassesRepository _studentsClassesrepository;
  public CreateBookingHandler(ITenantRepository tenantRepository, IBookingRepository bookingRepository, IClassDayRepository classDayRepository, IUserRepository userRepository, IUsersRolesRepository usersRolesRepository, ISubscriptionRepository subscriptionRepository, IStudentsClassesRepository studentsClassesRepository)
  {
    _tenantRepository = tenantRepository;
    _bookingRepository = bookingRepository;
    _classDayRepository = classDayRepository;
    _userRepository = userRepository;
    _usersRolesRepository = usersRolesRepository;
    _subscriptionRepository = subscriptionRepository;
    _studentsClassesrepository = studentsClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, CreateBookingCommand command)
  {

    command.Validate();

    if (!command.Valid)
    {
      return new CommandResult(false, "ERR_BOOKING_NOT_CREATED", null, command.Notifications);
    }

    var tenant = await _tenantRepository.GetByIdAndIncludePlanAsync(tenantId, new CancellationToken());

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_DAY_NOT_FOUND", null, null);
    }

    if (tenant.Status != ETenantStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var classDay = await _classDayRepository.GetByIdAsync(command.ClassDayId, new CancellationToken());

    if (classDay is null)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_FOUND", null, null, 404);
    }

    if (classDay.Status != EClassDayStatus.PENDING)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_PENDING", null, null, 403);
    }

    var user = await _userRepository.IdExistsAsync(command.UserId, new CancellationToken());

    if (!user)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null);
    }

    var userRole = await _usersRolesRepository.VerifyRoleExistsAsync(command.UserId, tenantId, "student", new CancellationToken());

    if (!userRole)
    {
      return new CommandResult(false, "ERR_STUDENT_ROLE_NOT_FOUND", null, 404);
    }

    var subscription = await _subscriptionRepository.GetLatestSubscription(tenantId, command.UserId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, 404);
    }

    if (subscription.Status != ESubscriptionStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_ACTIVE", null, 403);
    }

    var isClassStudent = await _studentsClassesrepository.GetByUserIdAndClassId(classDay.ClassId, command.UserId);

    if (isClassStudent is null)
    {
      return new CommandResult(false, "ERR_NOT_CLASS_STUDENT", null, 404);
    }

    var sunday = DateTime.Now.FirstDayOfWeek();
    var monday = DateTime.Now.LastDayOfWeek();

    var weekBookings = await _bookingRepository.GetAsync(x => x.UserId == command.UserId && x.ClassDay.Class.TenantId == tenantId && x.ClassDay.Date > sunday && x.ClassDay.Date < monday && x.ClassDay.Status != EClassDayStatus.CANCELED, new CancellationToken());

    if (weekBookings.Count() >= subscription.TenantPlan.TimesOfweek)
    {
      return new CommandResult(false, "ERR_WEEK_TIMES_EXCEEDED", null, 400);
    }

    var bookingAlreadyExists = await _bookingRepository.GetAsync(x => x.UserId == command.UserId && x.ClassDayId == command.ClassDayId, new CancellationToken());

    if (bookingAlreadyExists.Count() > 0)
    {
      return new CommandResult(false, "ERR_BOOKING_ALREADY_EXISTS", null, 409);
    }

    var booking = new Booking(command.UserId, command.ClassDayId);

    await _bookingRepository.CreateAsync(booking, new CancellationToken());

    var bookingFound = await _bookingRepository.GetWithInclude(command.UserId, booking.Id);

    return new CommandResult(true, "BOOKING_CREATED", bookingFound, null, 201);
  }
}