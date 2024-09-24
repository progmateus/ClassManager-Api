using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class CreateClassHandler :
  Notifiable,
  ITenantHandler<ClassCommand>
{
  private readonly IClassRepository _classRepository;
  private readonly ITenantRepository _tenantRepository;
  private readonly IAccessControlService _accessControlService;

  public CreateClassHandler(
    IClassRepository classRepository,
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService
    )
  {
    _classRepository = classRepository;
    _tenantRepository = tenantRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, ClassCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_PLAN_NOT_CREATED", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (await _accessControlService.HasUserRoleAsync(loggedUserId, tenantId, "admin"))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    if (await _classRepository.NameAlreadyExists(command.Name, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_CLASS_ALREADY_EXISTS", null, null, 409);
    }

    var newClass = new Class(command.Name, tenantId, command.BusinessHour, command.Description);

    await _classRepository.CreateAsync(newClass, new CancellationToken());

    return new CommandResult(true, "CLASS_CREATED", newClass, null, 201);
  }
}
