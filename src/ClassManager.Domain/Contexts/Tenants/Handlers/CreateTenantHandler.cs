using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
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
  private readonly IUserRepository _usersRepository;
  private readonly IRoleRepository _roleRepository;
  private readonly IUsersRolesRepository _usersRolesRepository;

  public CreateTenantHandler(
    ITenantRepository tenantRepository,
    IUserRepository usersRepository,
    IRoleRepository roleRepository,
    IUsersRolesRepository usersRolesRepository

    )
  {
    _repository = tenantRepository;
    _usersRepository = usersRepository;
    _roleRepository = roleRepository;
    _usersRolesRepository = usersRolesRepository;
  }
  public async Task<ICommandResult> Handle(CreateTenantCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (await _repository.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "Document already exists");
    }

    if (await _repository.UsernameAlreadyExistsAsync(command.Username, new CancellationToken()))
    {
      AddNotification("Username", "Username already exists");
    }

    if (await _usersRepository.UsernameAlreadyExistsAsync(command.Username, new CancellationToken()))
    {
      AddNotification("Username", "Username already exists");
    }

    if (await _repository.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
    {
      AddNotification("Email", "E-mail already exists");
    }

    var document = new Document(command.Document, EDocumentType.CNPJ);
    var email = new Email(command.Email);

    var tenant = new Tenant(command.Name, document, command.Username, command.Description, email, command.UserId);

    AddNotifications(document, email);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, Notifications);
    }

    var role = await _roleRepository.GetByNameAsync("admin", new CancellationToken());

    if (role is null)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_FOUND", null, null, 404);
    }

    await _repository.CreateAsync(tenant, new CancellationToken());

    var userRole = new UsersRoles(command.UserId, role.Id, tenant.Id);

    await _usersRolesRepository.CreateAsync(userRole, new CancellationToken());

    return new CommandResult(true, "TENANT_CREATED", tenant, null, 201);
  }
}
