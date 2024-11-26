using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class UpdateClassDayHandler :
  Notifiable,
  ITenantActionHandler<UpdateClassDayCommand>
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly ITeacherClassesRepository _teacherClassesrepository;


  public UpdateClassDayHandler(
    IClassDayRepository classDayRepository,
    IAccessControlService accessControlService,
    ITeacherClassesRepository teacherClassesRepository

    )
  {
    _classDayRepository = classDayRepository;
    _accessControlService = accessControlService;
    _teacherClassesrepository = teacherClassesRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classDayId, UpdateClassDayCommand command)
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

    var isAdmin = await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]);

    var classDay = await _classDayRepository.GetByIdAsync(classDayId, new CancellationToken());

    if (classDay is null)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_FOUND", null, null, 404);
    }

    if (!await _accessControlService.HasClassRoleAsync(loggedUserId, tenantId, classDay.ClassId, ["student", "teacher"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    if (await _teacherClassesrepository.GetByUserIdAndClassId(loggedUserId, classDay.ClassId) is null && !isAdmin)
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    if (classDay.Status != EClassDayStatus.PENDING)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_PENDING", null, null, 409);
    }

    classDay.ChangeStatus(command.Status, command.Observation);

    await _classDayRepository.UpdateAsync(classDay, new CancellationToken());

    return new CommandResult(true, "CLASS_DAY_UPDATED", classDay, null, 201);
  }
}
