using AutoMapper;
using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.Bookings.ViewModels;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClasManager.Domain.Contexts.Bookings.Handlers;

public class CreateBookingHandler : Notifiable, ITenantHandler<CreateBookingCommand>
{
  private IBookingRepository _bookingRepository;
  private IClassDayRepository _classDayRepository;
  private ISubscriptionRepository _subscriptionRepository;
  private IStudentsClassesRepository _studentsClassesrepository;
  private IAccessControlService _accessControlService;
  private IMapper _mapper;
  public CreateBookingHandler(
    IBookingRepository bookingRepository,
    IClassDayRepository classDayRepository,
    ISubscriptionRepository subscriptionRepository,
    IStudentsClassesRepository studentsClassesRepository,
    IAccessControlService accessControlService,
    IMapper mapper
  )
  {
    _bookingRepository = bookingRepository;
    _classDayRepository = classDayRepository;
    _subscriptionRepository = subscriptionRepository;
    _studentsClassesrepository = studentsClassesRepository;
    _accessControlService = accessControlService;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateBookingCommand command)
  {

    command.Validate();

    if (!command.Valid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var targetUserId = loggedUserId;

    if (command.UserId.HasValue)
    {
      if (await _accessControlService.CheckParameterUserIdPermission(tenantId, loggedUserId, command.UserId))
      {
        targetUserId = command.UserId.Value;
      }
      else
      {
        return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
      }
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
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

    if (DateTime.Now > classDay.Date)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_TIME_EXPIRED", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(targetUserId, tenantId, ["student"]))
    {
      return new CommandResult(false, "ERR_STUDENT_ROLE_NOT_FOUND", null, null, 403);
    }

    var subscription = await _subscriptionRepository.FindUserLatestSubscription(tenantId, targetUserId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, 404);
    }

    if (subscription.Status != ESubscriptionStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_ACTIVE", null, 403);
    }

    var isClassStudent = await _studentsClassesrepository.FindByUserIdAndClassId(classDay.ClassId, targetUserId);

    if (isClassStudent is null)
    {
      return new CommandResult(false, "ERR_NOT_CLASS_STUDENT", null, 404);
    }

    var sunday = DateTime.Now.FirstDayOfWeek();
    var monday = DateTime.Now.LastDayOfWeek();

    if (subscription.TenantPlan is null)
    {
      return new CommandResult(false, "ERR_TENANT_PLAN_NOT_FOUND", null, 404);
    }

    var weekBookings = await _bookingRepository.GetAsync(x => x.UserId == targetUserId && x.ClassDay.Class.TenantId == tenantId && x.ClassDay.Date > sunday && x.ClassDay.Date < monday && x.ClassDay.Status != EClassDayStatus.CANCELED, []);

    if (weekBookings.Count() >= subscription.TenantPlan.TimesOfweek)
    {
      return new CommandResult(false, "ERR_WEEK_TIMES_EXCEEDED", null, 400);
    }

    var bookingAlreadyExists = await _bookingRepository.GetAsync(x => x.UserId == targetUserId && x.ClassDayId == command.ClassDayId, []);

    if (bookingAlreadyExists.Count() > 0)
    {
      return new CommandResult(false, "ERR_BOOKING_ALREADY_EXISTS", null, 409);
    }

    var booking = new Booking(targetUserId, command.ClassDayId);

    await _bookingRepository.CreateAsync(booking, new CancellationToken());

    var bookingFound = _mapper.Map<BookingViewModel>(await _bookingRepository.FindAsync(x => x.Id == booking.Id, [x => x.User, x => x.ClassDay]));

    return new CommandResult(true, "BOOKING_CREATED", bookingFound, null, 201);
  }
}