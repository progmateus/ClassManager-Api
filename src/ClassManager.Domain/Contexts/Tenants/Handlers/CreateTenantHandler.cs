using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class CreateTenantHandler :
  Notifiable
{
  private readonly ITenantRepository _repository;
  private readonly IUserRepository _usersRepository;
  private readonly IRoleRepository _roleRepository;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IPaymentService _paymentService;
  private readonly IMapper _mapper;

  public CreateTenantHandler(
    ITenantRepository tenantRepository,
    IUserRepository usersRepository,
    IRoleRepository roleRepository,
    IUsersRolesRepository usersRolesRepository,
    IPaymentService paymentService,
    IMapper mapper

    )
  {
    _repository = tenantRepository;
    _usersRepository = usersRepository;
    _roleRepository = roleRepository;
    _usersRolesRepository = usersRolesRepository;
    _paymentService = paymentService;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, CreateTenantCommand command)
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

    var tenant = new Tenant(command.Name, document, command.Username, command.Description, email, loggedUserId);

    var stripeCreatedAccount = _paymentService.CreateAccount(tenant.Id, tenant.Email);

    tenant.SetStripeAccountId(stripeCreatedAccount.Id);

    await _repository.CreateAsync(tenant, new CancellationToken());

    var userRole = new UsersRoles(loggedUserId, role.Id, tenant.Id);

    await _usersRolesRepository.CreateAsync(userRole, new CancellationToken());

    var tenantCreated = _mapper.Map<TenantViewModel>(await _repository.FindAsync(x => x.Id == tenant.Id, [x => x.UsersRoles]));

    if (tenantCreated.UsersRoles.Count > 0)
    {
      tenantCreated.UsersRoles[0].Role = _mapper.Map<RoleViewModel>(role);
    }

    return new CommandResult(true, "TENANT_CREATED", tenantCreated, null, 201);
  }
}
