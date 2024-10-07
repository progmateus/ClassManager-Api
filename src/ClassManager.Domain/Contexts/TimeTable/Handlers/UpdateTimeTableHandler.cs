using ClassManager.Domain.Contexts.ClassDays.Helpers;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.Commands;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.TimesTabless.Handlers;

public class UpdateTimetableHandler :
  Notifiable
{
  private readonly ITimeTableRepository _timeTableRepository;
  private readonly IScheduleDayRepository _scheduleDayRepository;
  private IAccessControlService _accessControlService;
  private GenerateClassesDaysHelper _generateClassesDaysHelper;
  private IClassDayRepository _classDayRepository;

  public UpdateTimetableHandler(
    ITimeTableRepository timeTableRepository,
    IScheduleDayRepository scheduleDayRepository,
    IAccessControlService accessControlService,
    GenerateClassesDaysHelper generateClassesDaysHelper,
    IClassDayRepository classDayRepository
    )
  {
    _timeTableRepository = timeTableRepository;
    _scheduleDayRepository = scheduleDayRepository;
    _accessControlService = accessControlService;
    _generateClassesDaysHelper = generateClassesDaysHelper;
    _classDayRepository = classDayRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid timeTableId, UpdateTimeTableCommand command)
  {
    if (command.SchedulesDays.IsNullOrEmpty())
    {
      return new CommandResult(false, "ERR_VALIDATION", null, null, 400);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var timeTableExists = await _timeTableRepository.IdExistsAsync(timeTableId, tenantId, new CancellationToken());

    if (!timeTableExists)
    {
    }

    List<ScheduleDay> schedulesDaysEntities = [];

    foreach (var scheduleHour in command.SchedulesDays)
    {
      var scheduleDayEntity = new ScheduleDay(timeTableId, scheduleHour.WeekDay, scheduleHour.HourStart, scheduleHour.HourEnd, tenantId);
      schedulesDaysEntities.Add(scheduleDayEntity);
    }

    await _scheduleDayRepository.DeleteAllByTimeTableId(timeTableId, new CancellationToken());

    await _scheduleDayRepository.CreateRangeAsync(schedulesDaysEntities, new CancellationToken());


    var lastDayOfMonth = DateTime.Now.GetLastDayOfMonth().AddHours(23).AddMinutes(59).AddSeconds(59);

    var timeTableReloaded = await _timeTableRepository.FindAsync(x => x.Id == timeTableId && x.TenantId == tenantId, [x => x.Classes, x => x.SchedulesDays]);

    if (timeTableReloaded is null)
    {
      return new CommandResult(false, "ERR_TIME_TABLE_NOT_FOUND", null, null, 404);
    }

    var classesIds = timeTableReloaded.Classes.Select(c => c.Id).ToList();

    await _classDayRepository.DeleteAllAfterAndBeforeDate(classesIds, DateTime.Now, lastDayOfMonth, new CancellationToken());

    await _generateClassesDaysHelper.Handle([timeTableReloaded], DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

    return new CommandResult(true, "TIME_TABLE_UPDATED", "", null, 200);
  }
}
