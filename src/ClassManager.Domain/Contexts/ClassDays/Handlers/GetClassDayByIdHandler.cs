using AutoMapper;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class GetClassDayByIdHandler
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;
  private readonly IBookingRepository _bookingsRepository;
  public GetClassDayByIdHandler(
    IClassDayRepository classRepository,
    IMapper mapper,
    IAccessControlService accessControlService,
    IBookingRepository bookingRepository
    )
  {
    _classDayRepository = classRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
    _bookingsRepository = bookingRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classDayId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin", "student", "teacher"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var classDay = _mapper.Map<ClassDayViewModel>(await _classDayRepository.FindClassDayProfile(tenantId, classDayId));

    if (classDay is null)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_FOUND", null, null, 404);
    }

    classDay.BookingsCount = await _bookingsRepository.CountByClassDay(tenantId, classDay.Id, default);

    if (!await _accessControlService.HasClassRoleAsync(loggedUserId, tenantId, classDay.ClassId, ["student", "teacher"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    return new CommandResult(true, "CLASS_DAY_GOTTEN", classDay, null, 200);
  }
}
