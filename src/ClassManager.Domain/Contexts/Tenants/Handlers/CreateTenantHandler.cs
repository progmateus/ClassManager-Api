using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
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
  private readonly IPaymentService _paymentService;
  private readonly IMapper _mapper;
  private readonly IPlanRepository _planRepository;
  private readonly ISubscriptionRepository _subscriptionsRepository;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;
  private readonly IUsersRolesRepository _usersRolesRepository;

  public CreateTenantHandler(
    ITenantRepository tenantRepository,
    IUserRepository usersRepository,
    IRoleRepository roleRepository,
    IPaymentService paymentService,
    IMapper mapper,
    IPlanRepository planRepository,
    ISubscriptionRepository subscriptionsRepository,
    IStripeCustomerRepository stripeCustomerRepository,
    IUsersRolesRepository usersRolesRepository

    )
  {
    _tenantRepository = tenantRepository;
    _usersRepository = usersRepository;
    _roleRepository = roleRepository;
    _paymentService = paymentService;
    _mapper = mapper;
    _planRepository = planRepository;
    _subscriptionsRepository = subscriptionsRepository;
    _stripeCustomerRepository = stripeCustomerRepository;
    _usersRolesRepository = usersRolesRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, string userIpAddress, CreateTenantCommand command)
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

    var document = new Document(command.Document);
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

    var plan = await _planRepository.GetByIdAsync(command.PlanId, new CancellationToken());

    if (plan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    var tenant = new Tenant(command.Name, document, command.Username, command.Description, email, loggedUserId, command.PlanId);

    var subscription = new Subscription(tenant.Id, plan.Id);

    var userAdminRole = new UsersRoles(loggedUserId, role.Id, tenant.Id);

    var stripeCreatedAccount = _paymentService.CreateAccount(email);

    var stripeCreatedCustomer = _paymentService.CreateCustomer(tenant.Name, tenant.Email, null);

    _paymentService.AcceptStripeTerms(userIpAddress, stripeCreatedAccount.Id);

    tenant.SetStripeInformations(stripeCreatedAccount.Id, stripeCreatedCustomer.Id);

    var stripeCustomerEntity = new StripeCustomer(stripeCreatedCustomer.Id, tenant.Id, loggedUserId, ETargetType.TENANT);

    tenant.UsersRoles.Add(userAdminRole);
    tenant.StripeCustomers.Add(stripeCustomerEntity);

    await _tenantRepository.CreateAsync(tenant, new CancellationToken());

    var stripeSubscription = _paymentService.CreateSubscription(subscription.Id, null, tenant.Id, plan.StripePriceId, stripeCreatedCustomer.Id, ETargetType.TENANT, null);

    subscription.SetStripeSubscriptionId(stripeSubscription.Id);
    subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);

    await _subscriptionsRepository.CreateAsync(subscription, new CancellationToken());

    _paymentService.CreateInvoice(null, null, tenant.Id, stripeCreatedCustomer.Id, stripeSubscription.Id, null);


    var tenantCreated = _mapper.Map<TenantViewModel>(tenant);

    if (tenantCreated.UsersRoles.Count > 0)
    {
      tenantCreated.UsersRoles[0].Role = _mapper.Map<RoleViewModel>(role);
    }

    return new CommandResult(true, "TENANT_CREATED", tenantCreated, null, 201);
  }
}
