using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class CreateRoleHandler : Notifiable,
  IHandler<RoleCommand>
{
  private IRoleRepository _repository;
  public CreateRoleHandler(IRoleRepository roleRepository)
  {
    _repository = roleRepository;
  }
  public async Task<ICommandResult> Handle(RoleCommand command)
  {
    command.Validate();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var roleAlreadyExists = await _repository.NameAlreadyExists(command.Name, new CancellationToken());

    if (roleAlreadyExists)
    {
      return new CommandResult(false, "ERR_ROLE_ALREADY_EXISTS", null, null, 409);
    }

    var role = new Role(command.Name, command.Description);

    await _repository.CreateAsync(role, new CancellationToken());

    return new CommandResult(false, "ROLE_CREATED", null, null, 409);
  }
}