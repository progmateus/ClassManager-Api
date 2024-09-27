using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class UpdateRoleHandler : Notifiable,
  IActionHandler<RoleCommand>
{
  private IRoleRepository _repository;
  public UpdateRoleHandler(IRoleRepository roleRepository)
  {
    _repository = roleRepository;
  }
  public async Task<ICommandResult> Handle(Guid id, RoleCommand command)
  {
    command.Validate();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var role = await _repository.GetByIdAsync(id, new CancellationToken());

    if (role is null)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_FOUND", null, null, 404);
    }

    role.ChangeRole(command.Name, command.Description);

    await _repository.UpdateAsync(role, new CancellationToken());

    return new CommandResult(false, "ROLE_EDITED", null, null, 200);
  }
}