using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimeTables.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.TimeTables.Handlers;

public class UpdateTimetableHandler :
  Notifiable
{
  private readonly ITimeTableRepository _timeTableRepository;
  private readonly IScheduleRepository _scheduleRepository;

  public UpdateTimetableHandler(
    ITimeTableRepository classHourRepository,
    IScheduleRepository scheduleRepository
    )
  {
    _timeTableRepository = classHourRepository;
    _scheduleRepository = scheduleRepository;
  }
  public async Task<ICommandResult> Handle(Guid timeTableId, UpdateTimeTableCommand command, Guid tenantId)
  {
    if (command.ScheduleDays.IsNullOrEmpty())
    {
      return new CommandResult(false, "ERR_INVALID_CLASS_HOUR", null, null, 400);
    }


    if (!await _timeTableRepository.IdExistsAsync(timeTableId, tenantId, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_TIME_TABLE_NOT_FOUND", null, null, 404);
    }

    var grouped = command.ScheduleDays
    .GroupBy(ch => ch.WeekDay)
    .Select(grp => grp.ToList())
    .ToList();

    List<ScheduleDay> classesHoursEntities = [];

    foreach (var classHour in command.ScheduleDays)
    {
      var classHourEntity = new ScheduleDay(timeTableId, classHour.WeekDay, classHour.HourStart, classHour.HourEnd, tenantId);
      classesHoursEntities.Add(classHourEntity);
    }

    await _scheduleRepository.DeleteAllByTenantIdAsync(tenantId, new CancellationToken());

    await _scheduleRepository.CreateRangeAsync(classesHoursEntities, new CancellationToken());

    return new CommandResult(true, "CLASS_HOUR_CREATED", "", null, 201);
  }
}
