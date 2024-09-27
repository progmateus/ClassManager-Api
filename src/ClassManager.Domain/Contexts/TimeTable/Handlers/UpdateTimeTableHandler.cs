using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
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

  public UpdateTimetableHandler(
    ITimeTableRepository classHourRepository,
    IScheduleDayRepository scheduleDayRepository,
    IAccessControlService accessControlService
    )
  {
    _timeTableRepository = classHourRepository;
    _scheduleDayRepository = scheduleDayRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid timeTableId, UpdateTimeTableCommand command)
  {
    if (command.ScheduleDays.IsNullOrEmpty())
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

    if (!await _timeTableRepository.IdExistsAsync(timeTableId, tenantId, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_TIME_TABLE_NOT_FOUND", null, null, 404);
    }

    List<ScheduleDay> schedulesDaysEntities = [];

    foreach (var classHour in command.ScheduleDays)
    {
      var classHourEntity = new ScheduleDay(timeTableId, classHour.WeekDay, classHour.HourStart, classHour.HourEnd, tenantId);
      schedulesDaysEntities.Add(classHourEntity);
    }

    await _scheduleDayRepository.DeleteAllByTimeTableId(timeTableId, new CancellationToken());

    await _scheduleDayRepository.CreateRangeAsync(schedulesDaysEntities, new CancellationToken());

    return new CommandResult(true, "TIME_TABLE_UPDATED", "", null, 200);
  }
}
