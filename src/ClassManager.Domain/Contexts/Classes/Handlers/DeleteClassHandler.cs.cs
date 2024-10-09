using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class DeleteClassHandler : ITenantDeleteAction
{
  private readonly IClassRepository _classRepository;
  private readonly IAccessControlService _accessControlService;

  public DeleteClassHandler(
    IClassRepository classRepository,
    IAccessControlService accessControlService

  )
  {
    _classRepository = classRepository;
    _accessControlService = accessControlService;
  }

  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid entityId)
  {
    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    if (await _classRepository.GetByIdAndTenantIdAsync(tenantId, entityId, new CancellationToken()) == null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }
    await _classRepository.DeleteAsync(entityId, tenantId, default);

    return new CommandResult(true, "CLASS_DELETED", null, null, 204);
  }
}
