using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class ListTenantsHandler
{
  private readonly ITenantRepository _repository;
  public ListTenantsHandler(
    ITenantRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }
  public async Task<ICommandResult> Handle()
  {
    var tenants = await _repository.GetAllAsync(default);

    return new CommandResult(true, "Tenants listed", tenants, null, 201);
  }
}
