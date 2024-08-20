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
  public async Task<ICommandResult> Handle(string search = "")
  {
    var tenants = await _repository.SearchAsync(search);

    return new CommandResult(true, "TENANTS_LISTED", tenants, null, 201);
  }
}
