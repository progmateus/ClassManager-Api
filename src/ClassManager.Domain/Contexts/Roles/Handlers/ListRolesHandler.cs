using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class ListRolesHandler
{
  private IRoleRepository _repository;
  public ListRolesHandler(IRoleRepository roleRepository)
  {
    _repository = roleRepository;
  }
  public async Task<ICommandResult> Handle()
  {
    var roles = await _repository.GetAllAsync(new CancellationToken());

    return new CommandResult(false, "ROLES_LISTED", roles, null, 200);
  }
}