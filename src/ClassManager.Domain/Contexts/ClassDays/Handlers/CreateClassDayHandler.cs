using ClassManager.Domain.Contexts.ClassDays.Commands;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class CreateClassDayHandler :
  Notifiable,
  ITenantHandler<CreateClassDayCommand>
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IClassRepository _classRepository;
  private readonly IAccessControlService _accessControlService;

  public CreateClassDayHandler(
    IClassDayRepository classDayRepository,
    IClassRepository classRepository,
    IAccessControlService accessControlService
    )
  {
    _classDayRepository = classDayRepository;
    _accessControlService = accessControlService;
    _classRepository = classRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateClassDayCommand command)
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

    if (!await _classRepository.IdExistsAsync(command.ClassId, tenantId, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var classDay = new ClassDay(command.Name, command.Date, command.HourStart, command.HourEnd, command.ClassId);

    await _classDayRepository.CreateAsync(classDay, new CancellationToken());

    return new CommandResult(true, "CLASS_DAY_CREATED", classDay, null, 201);
  }
}
