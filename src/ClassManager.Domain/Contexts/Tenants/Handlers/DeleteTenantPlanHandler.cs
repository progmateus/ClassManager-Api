using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class DeleteTenantPlanHandler : ITenantDeleteAction
{
  private readonly ITenantPlanRepository _tenantPlansRepository;
  private readonly IAccessControlService _accessControlService;

  public DeleteTenantPlanHandler(
    ITenantPlanRepository tenantPlanRepository,
    IAccessControlService accessControlService

    )
  {
    _tenantPlansRepository = tenantPlanRepository;
    _accessControlService = accessControlService;

  }

  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid entityId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserRoleAsync(loggedUserId, tenantId, "admin"))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    if (await _tenantPlansRepository.GetByIdAndTenantId(tenantId, entityId, new CancellationToken()) == null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    await _tenantPlansRepository.DeleteAsync(entityId, default);

    return new CommandResult(true, "PLAN_DELETED", null, null, 204);
  }
}
