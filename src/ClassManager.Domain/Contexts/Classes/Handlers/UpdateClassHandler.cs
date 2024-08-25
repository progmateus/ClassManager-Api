using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateClassHandler :
  Notifiable,
  ITenantActionHandler<ClassCommand>
{
  private readonly IClassRepository _classRepository;

  public UpdateClassHandler(
    IClassRepository classRepository
    )
  {
    _classRepository = classRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid classId, ClassCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_CLASS_NOT_UPDATED", null, command.Notifications);
    }
    var classFound = await _classRepository.GetByIdAndTenantId(tenantId, classId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }
    classFound.ChangeClass(command.Name, command.BusinessHour, command.Description);
    await _classRepository.UpdateAsync(classFound, new CancellationToken());

    return new CommandResult(true, "CLASS_UPDATED", classFound, null, 200);
  }
}
