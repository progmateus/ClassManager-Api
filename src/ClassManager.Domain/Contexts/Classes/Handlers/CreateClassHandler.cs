using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class CreateClassHandler :
  Notifiable,
  IActionHandler<ClassCommand>
{
  private readonly IClassRepository _classRepository;
  private readonly ITenantRepository _tenantRepository;

  public CreateClassHandler(
    IClassRepository classRepository,
    ITenantRepository tenantRepository
    )
  {
    _classRepository = classRepository;
    _tenantRepository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, ClassCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_PLAN_NOT_CREATED", null, command.Notifications);
    }
    var tenant = await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken());
    if (tenant)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    if (await _classRepository.NameAlreadyExists(command.Name, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_CLASS_ALREADY_EXISTS", null, null, 409);
    }

    var newClass = new Class(command.Name, tenantId, command.BusinessHour);

    await _classRepository.CreateAsync(newClass, new CancellationToken());

    return new CommandResult(true, "CLASS_CREATED", newClass, null, 201);
  }
}
