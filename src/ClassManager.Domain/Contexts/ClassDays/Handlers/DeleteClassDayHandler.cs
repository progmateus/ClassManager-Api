using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class DeleteClassDayHandler :
  Notifiable,
  ITenantDeleteAction
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IAccessControlService _accessControlService;


  public DeleteClassDayHandler(
    IClassDayRepository classDayRepository,
    IAccessControlService accessControlService

    )
  {
    _classDayRepository = classDayRepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classDayId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var classDay = await _classDayRepository.GetByIdAsync(classDayId, new CancellationToken());

    if (classDay is null)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_FOUND", null, null, 404);
    }

    await _classDayRepository.DeleteAsync(classDay.Id, new CancellationToken());

    return new CommandResult(true, "CLASS_DAY_DELETED", "", null, 204);
  }
}
