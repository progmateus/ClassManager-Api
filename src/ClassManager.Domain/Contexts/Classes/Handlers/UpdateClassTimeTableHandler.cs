using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Libs.MassTransit.Events;
using ClassManager.Domain.Libs.MassTransit.Publish;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateClassTimeTableHandler :
  Notifiable,
  ITenantActionHandler<UpdateClassTimeTableCommand>
{
  private readonly IClassRepository _classRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly ITimeTableRepository _timeTableRepository;
  private readonly IClassDayRepository _classDayRepository;
  private IPublishBus _publishBus;

  public UpdateClassTimeTableHandler(
    IClassRepository classRepository,
    IAccessControlService accessControlService,
    ITimeTableRepository timeTableRepository,
    IClassDayRepository classDayRepository,
    IPublishBus publishBus
    )
  {
    _classRepository = classRepository;
    _accessControlService = accessControlService;
    _timeTableRepository = timeTableRepository;
    _classDayRepository = classDayRepository;
    _publishBus = publishBus;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classId, UpdateClassTimeTableCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var classEntity = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classEntity is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var newTimeTable = await _timeTableRepository.FindByIdAndTenantIdAsync(command.TimeTableId, tenantId, new CancellationToken());

    if (newTimeTable is null || newTimeTable.TenantId != classEntity.TenantId)
    {
      return new CommandResult(false, "ERR_TIME_TABLE_NOT_FOUND", null, null, 404);
    }

    if (classEntity.TimeTableId.Equals(command.TimeTableId))
    {
      return new CommandResult(false, "ERR_CHOOSE_ANOTHER_TIME_TABLE", null, null, 404);
    }

    await _classDayRepository.DeleteAllAfterAndBeforeDate([classId], DateTime.Now, DateTime.Now.GetLastDayOfMonth().AddHours(23).AddMinutes(59).AddSeconds(59), new CancellationToken());

    classEntity.UpdateTimeTable(command.TimeTableId);

    await _classRepository.UpdateAsync(classEntity, new CancellationToken());

    var eventRequest = new RefreshClassClassesDaysEvent(classId);

    await _publishBus.PublicAsync(eventRequest);

    return new CommandResult(true, "CLASS_TIME_TABLE_UPDATED", classEntity, null, 200);
  }
}
