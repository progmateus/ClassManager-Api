using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class GetTenantHandler
{
  private readonly ITenantRepository _repository;
  public GetTenantHandler(
    ITenantRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(Guid id)
  {
    var tenant = await _repository.GetByIdAsync(id, new CancellationToken());

    return new CommandResult(true, "TENANT", tenant, null, 201);
  }
}
