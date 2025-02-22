using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.Commands;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using ClassManager.Domain.Libs.MassTransit.Events;
using ClassManager.Domain.Libs.MassTransit.Publish;
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
  private IClassDayRepository _classDayRepository;
  private IPublishBus _publishBus;

  public UpdateTimetableHandler(
    ITimeTableRepository timeTableRepository,
    IScheduleDayRepository scheduleDayRepository,
    IAccessControlService accessControlService,
    IClassDayRepository classDayRepository,
    IPublishBus publishBus
    )
  {
    _timeTableRepository = timeTableRepository;
    _scheduleDayRepository = scheduleDayRepository;
    _accessControlService = accessControlService;
    _classDayRepository = classDayRepository;
    _publishBus = publishBus;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid timeTableId, UpdateTimeTableCommand command)
  {

    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var timeTable = await _timeTableRepository.FindAsync(x => x.Id == timeTableId && x.TenantId == tenantId, [x => x.Classes]);

    if (timeTable is null)
    {
      return new CommandResult(false, "ERR_TIME_TABLE_NOT_FOUND", null, null, 404);
    }

    List<ScheduleDay> schedulesDaysEntities = [];

    await _scheduleDayRepository.DeleteAllByTimeTableId(timeTableId, new CancellationToken());

    foreach (var scheduleHour in command.SchedulesDays)
    {
      var scheduleDayEntity = new ScheduleDay(scheduleHour.Name, timeTableId, scheduleHour.WeekDay, scheduleHour.HourStart, scheduleHour.HourEnd, tenantId);
      schedulesDaysEntities.Add(scheduleDayEntity);
    }

    timeTable.SetName(command.Name);

    await _timeTableRepository.UpdateAsync(timeTable, new CancellationToken());

    await _scheduleDayRepository.CreateRangeAsync(schedulesDaysEntities, new CancellationToken());

    var lastDayOfMonth = DateTime.Now.GetLastDayOfMonth().AddHours(23).AddMinutes(59).AddSeconds(59);

    var classesIds = timeTable.Classes.Select(c => c.Id).ToList();

    await _classDayRepository.DeleteAllAfterAndBeforeDate(classesIds, DateTime.Now, lastDayOfMonth, new CancellationToken());

    if (schedulesDaysEntities.Count > 0)
    {
      var eventRequest = new GeneratedClassesDaysEvent([timeTableId], DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

      await _publishBus.PublicAsync(eventRequest);
    }
    return new CommandResult(true, "TIME_TABLE_UPDATED", "", null, 200);
  }
}
