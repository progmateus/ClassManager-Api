using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class DeleteTenantPlanHandler
{
  private readonly ITenantPlanRepository _repository;
  public DeleteTenantPlanHandler(
    ITenantPlanRepository tenantPlanRepository
    )
  {
    _repository = tenantPlanRepository;
  }

  public async Task<ICommandResult> Handle(Guid tenantId, Guid id)
  {
    Console.WriteLine("AQUI");
    if (await _repository.GetByIdAndTenantId(tenantId, id, new CancellationToken()) == null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }
    Console.WriteLine("VAI DELETAR");
    await _repository.DeleteAsync(id, default);

    return new CommandResult(true, "PLAN_DELETED", null, null, 204);
  }
}
