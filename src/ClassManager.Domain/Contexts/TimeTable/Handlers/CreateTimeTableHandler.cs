using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.Commands;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.TimesTables.Handlers;

public class CreateTimeTableHandler :
  Notifiable, ITenantHandler<CreateTimeTableCommand>
{
  private readonly ITimeTableRepository _timeTableRepository;
  private IAccessControlService _accessControlService;

  public CreateTimeTableHandler(
    ITimeTableRepository classHourRepository,
    IAccessControlService accessControlService
    )
  {
    _timeTableRepository = classHourRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateTimeTableCommand command)
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

    var timeTable = new TimeTable(tenantId, command.Name);


    await _timeTableRepository.CreateAsync(timeTable, new CancellationToken());

    return new CommandResult(true, "TIME_TABLE_CREATED", timeTable, null, 201);
  }
}
