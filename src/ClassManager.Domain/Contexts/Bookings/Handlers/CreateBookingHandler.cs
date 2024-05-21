using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;

namespace ClasManager.Domain.Contexts.Bookings.Handlers;

public class CreateBookingHandler : Notifiable
{
  private ITenantRepository _tenantRepository;
  private IBookingRepository _bookingRepository;
  private IClassDayRepository _classDayRepository;
  private IUserRepository _userRepository;
  public CreateBookingHandler(ITenantRepository tenantRepository, IBookingRepository bookingRepository, IClassDayRepository classDayRepository, IUserRepository userRepository)
  {
    _tenantRepository = tenantRepository;
    _bookingRepository = bookingRepository;
    _classDayRepository = classDayRepository;
    _userRepository = userRepository;
  }
  public async Task<CommandResult> Handle(Guid tenantId, Guid userId, Guid classDayId)
  {

    var tenant = await _tenantRepository.GetByIdAndIncludePlanAsync(tenantId, new CancellationToken());

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_DAY_NOT_FOUND", null, null);
    }

    if (tenant.Status != ETenantStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var classDay = await _classDayRepository.GetByIdAsync(classDayId, new CancellationToken());

    if (classDay is null)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_FOUND", null, null);
    }

    var user = await _userRepository.GetRolesByIdAsync(userId, new CancellationToken());

    if (user is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null);
    }

    if (user.UsersRoles.Where(ur => ur.Role.Name == "student" && ur.TenantId == tenantId).ToList() is null)
    {
      return new CommandResult(false, "ERR_STUDENT_ROLE_NOT_FOUND", null, 404);
    }

    var sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday);

    var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

    Console.WriteLine("=========================================================");
    Console.WriteLine("sunday: ", sunday);
    Console.WriteLine("monday: ", monday);
    Console.WriteLine("=========================================================");


    var booking = new Booking(userId, classDayId);

    await _bookingRepository.CreateAsync(booking, new CancellationToken());

    return new CommandResult(true, "BOOKING_CREATED", booking, null, 201);
  }
}