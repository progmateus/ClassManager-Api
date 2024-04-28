using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class UpdateTenantHandler :
  Notifiable,
  IActionHandler<CreateTenantCommand>
{
  private readonly ITenantRepository _repository;

  public UpdateTenantHandler(
    ITenantRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(Guid id, CreateTenantCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "Tenant not Updated", null, command.Notifications);
    }

    var tenant = await _repository.GetByIdAsync(id, default);

    if (tenant is null)
    {
      return new CommandResult(false, "Tenant not found", null, command.Notifications, 404);
    }


    if ((tenant.Document.Number != command.Document) && await _repository.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "Docuemnt already exists");
    }


    if ((tenant.Email.Address != command.Email.ToLower()) && await _repository.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
    {
      AddNotification("Email", "E-mail already exists");
    }

    var document = new Document(command.Document, EDocumentType.CPF);
    var email = new Email(command.Email);

    tenant.ChangeTenant(command.Name, email, document);

    // agrupar validações

    AddNotifications(document, email, tenant);

    if (Invalid)
    {
      return new CommandResult(false, "Tenant not created", null, Notifications);
    }

    await _repository.UpdateAsync(tenant, default);

    return new CommandResult(true, "Tenant created", tenant, null, 201);
  }
}
