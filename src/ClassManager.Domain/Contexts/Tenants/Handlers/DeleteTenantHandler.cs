using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class DeleteTenantHandler : Notifiable
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IAccessControlService _accessControlService;

  public DeleteTenantHandler(
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService

    )
  {
    _tenantRepository = tenantRepository;
    _accessControlService = accessControlService;

  }

  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    tenant.UpdateStatus(ETenantStatus.DELETED);

    await _tenantRepository.UpdateAsync(tenant, default);

    return new CommandResult(true, "TENANT_DELETED", null, null, 204);
  }
}
