using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class DeleteClassHandler
{
  private readonly IClassRepository _classRepository;
  public DeleteClassHandler(
    IClassRepository classRepository
    )
  {
    _classRepository = classRepository;
  }

  public async Task<ICommandResult> Handle(Guid tenantId, Guid id)
  {
    if (await _classRepository.GetByIdAndTenantIdAsync(tenantId, id, new CancellationToken()) == null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }
    await _classRepository.DeleteAsync(id, default);

    return new CommandResult(true, "CLASS_DELETED", null, null, 204);
  }
}
