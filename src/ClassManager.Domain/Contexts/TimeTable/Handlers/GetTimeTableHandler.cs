using AutoMapper;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.TimesTabless.Handlers;

public class GetTimeTableHandler
{
  private readonly ITimeTableRepository _timeTableRepository;
  private readonly IScheduleDayRepository _scheduleDayRepository;
  private IAccessControlService _accessControlService;
  private IMapper _mapper;

  public GetTimeTableHandler(
    ITimeTableRepository classHourRepository,
    IAccessControlService accessControlService,
    IScheduleDayRepository scheduleDayRepository,
    IMapper mapper
    )
  {
    _timeTableRepository = classHourRepository;
    _accessControlService = accessControlService;
    _scheduleDayRepository = scheduleDayRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid entityId)
  {

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var timeTable = _mapper.Map<TimeTableViewModel>(await _timeTableRepository.FindAsync(x => x.TenantId == tenantId && x.Id == entityId, [x => x.SchedulesDays]));

    if (timeTable is null)
    {
      return new CommandResult(false, "ERR_TIME_TABLE_NOT_FOUND", null, null, 404);
    }

    /*     var shchedulesDays = _mapper.Map<List<ScheduleDayViwModel>>(await _scheduleDayRepository.GroupByWeekDay(entityId)); ;
      timeTable.SchedulesDays = shchedulesDays;
    */

    return new CommandResult(true, "TIME_TABLE_GOTTENNN", timeTable, null, 200);
  }
}
