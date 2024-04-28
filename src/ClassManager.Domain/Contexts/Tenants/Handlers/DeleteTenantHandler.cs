using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class DeleteTenantHandler : Notifiable
{
  private readonly ITenantRepository _repository;
  public DeleteTenantHandler(
    ITenantRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }

  public async Task<ICommandResult> Handle(Guid id)
  {

    if (await _repository.GetByIdAsync(id, default) == null)
    {
      AddNotification("DeleteTenantHandler", "Tenant not found");
      return new CommandResult(false, "Tenant not deleted", null, Notifications, 404);
    }


    await _repository.DeleteAsync(id, default);

    return new CommandResult(true, "Tenant deleted", null, null, 204);
  }
}
