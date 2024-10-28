using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class DeleteRoleHandler
{
  private IRoleRepository _repository;
  public DeleteRoleHandler(IRoleRepository roleRepository)
  {
    _repository = roleRepository;
  }
  public async Task<ICommandResult> Handle(Guid id)
  {
    var role = await _repository.GetByIdAsync(id, new CancellationToken());

    if (role is null)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_FOUND", null, null, 404);
    }

    await _repository.DeleteAsync(id, new CancellationToken());

    return new CommandResult(false, "ROLE_DELETED", null, null, 200);
  }
}