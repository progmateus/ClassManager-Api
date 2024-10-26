using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class CreateTenantHandler :
  Notifiable
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IUserRepository _usersRepository;
  private readonly IRoleRepository _roleRepository;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IPaymentService _paymentService;
  private readonly IMapper _mapper;
  private readonly IPlanRepository _planRepository;

  public CreateTenantHandler(
    ITenantRepository tenantRepository,
    IUserRepository usersRepository,
    IRoleRepository roleRepository,
    IUsersRolesRepository usersRolesRepository,
    IPaymentService paymentService,
    IMapper mapper,
    IPlanRepository planRepository

    )
  {
    _tenantRepository = tenantRepository;
    _usersRepository = usersRepository;
    _roleRepository = roleRepository;
    _usersRolesRepository = usersRolesRepository;
    _paymentService = paymentService;
    _mapper = mapper;
    _planRepository = planRepository;
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

    if (await _tenantRepository.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "Document already exists");
    }

    if (await _tenantRepository.UsernameAlreadyExistsAsync(command.Username, new CancellationToken()))
    {
      AddNotification("Username", "Username already exists");
    }

    if (await _usersRepository.UsernameAlreadyExistsAsync(command.Username, new CancellationToken()))
    {
      AddNotification("Username", "Username already exists");
    }

    if (await _tenantRepository.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
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

    var tenantPlan = await _planRepository.GetByIdAsync(command.PlanId, new CancellationToken());

    if (tenantPlan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    var tenant = new Tenant(command.Name, document, command.Username, command.Description, email, loggedUserId);

    var stripeCreatedAccount = _paymentService.CreateAccount(tenant.Id, tenant.Email);
    var stripeCreatedCustomer = _paymentService.CreateCustomer(tenant.Name, tenant.Email, null);
    var stripeSubscription = _paymentService.CreateSubscription(null, tenantPlan.StripePriceId, stripeCreatedCustomer.Id, null);
    var stripeInvoice = _paymentService.CreateInvoice(tenant.Id, stripeCreatedCustomer.Id, stripeSubscription.Id, null);

    tenant.SetStripeInformations(stripeCreatedAccount.Id, stripeCreatedCustomer.Id, stripeSubscription.Id);

    var invoice = new Invoice(loggedUserId, null, null, tenantPlan.Id, tenant.Id, tenantPlan.Price, EInvoiceTargetType.TENANT, EInvoiceType.TENANT_SUBSCRIPTION);
    invoice.SetStripeInformations(stripeInvoice.Id, stripeInvoice.HostedInvoiceUrl, stripeInvoice.Number);

    var userAdminRole = new UsersRoles(loggedUserId, role.Id, tenant.Id);

    tenant.Invoices.Add(invoice);
    /* tenant.UsersRoles.Add(userAdminRole); */

    await _tenantRepository.CreateAsync(tenant, new CancellationToken());

    var tenantCreated = _mapper.Map<TenantViewModel>(tenant);

    if (tenantCreated.UsersRoles.Count > 0)
    {
      tenantCreated.UsersRoles[0].Role = _mapper.Map<RoleViewModel>(role);
    }

    return new CommandResult(true, "TENANT_CREATED", tenantCreated, null, 201);
  }
}
