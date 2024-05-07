using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class CreateTenantHandler :
  Notifiable,
  IHandler<CreateTenantCommand>
{
  private readonly ITenantRepository _repository;

  public CreateTenantHandler(
    ITenantRepository tenantRepository
    )
  {
    _repository = tenantRepository;
  }
  public async Task<ICommandResult> Handle(CreateTenantCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "Tenant not Created", null, command.Notifications);
    }

    if (await _repository.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "Docuemnt already exists");
    }

    if (await _repository.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
    {
      AddNotification("Email", "E-mail already exists");
    }

    var document = new Document(command.Document, EDocumentType.CNPJ);
    var email = new Email(command.Email);

    var tenant = new Tenant(command.Name, document, email);


    AddNotifications(document, email);

    if (Invalid)
    {
      return new CommandResult(false, "Tenant not created", null, Notifications);
    }


    await _repository.CreateAsync(tenant, new CancellationToken());

    return new CommandResult(true, "User created", tenant, null, 201);
  }
}
