using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class CreateTenantBankAccountHandler :
  Notifiable,
  ITenantHandler<CreateTenantBankAccountCommand>
{
  private readonly ITenantRepository _repository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentSetvice;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;

  public CreateTenantBankAccountHandler(
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IStripeCustomerRepository stripeCustomerRepository

    )
  {
    _repository = tenantRepository;
    _accessControlService = accessControlService;
    _paymentSetvice = paymentService;
    _stripeCustomerRepository = stripeCustomerRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateTenantBankAccountCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var tenant = await _repository.GetByIdAsync(tenantId, default);

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, command.Notifications, 404);
    }

    var stripeCustomer = await _stripeCustomerRepository.FindByUserIdAndTenantIdAndType(loggedUserId, tenantId, ETargetType.TENANT, default);

    if (stripeCustomer is null)
    {
      return new CommandResult(false, "ERR_STRIPE_CUSTOMER_NOT_FOUND", null, null, 404);
    }

    _paymentSetvice.CreateBankAccount(command.Number, command.Country, command.Currency, command.AccountHolderName, tenant.StripeAccountId);

    return new CommandResult(true, "BANK_ACCOUNT_CREATED", new { }, null, 201);
  }
}
