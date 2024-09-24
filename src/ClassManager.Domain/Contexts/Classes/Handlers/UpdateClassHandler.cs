using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateClassHandler :
  Notifiable,
  ITenantActionHandler<ClassCommand>
{
  private readonly IClassRepository _classRepository;
  private readonly IAccessControlService _accessControlService;

  public UpdateClassHandler(
    IClassRepository classRepository,
    IAccessControlService accessControlService
    )
  {
    _classRepository = classRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classId, ClassCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_CLASS_NOT_UPDATED", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (await _accessControlService.HasUserRoleAsync(loggedUserId, tenantId, "admin"))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }
    classFound.ChangeClass(command.Name, command.BusinessHour, command.Description);
    await _classRepository.UpdateAsync(classFound, new CancellationToken());

    return new CommandResult(true, "CLASS_UPDATED", classFound, null, 200);
  }
}
