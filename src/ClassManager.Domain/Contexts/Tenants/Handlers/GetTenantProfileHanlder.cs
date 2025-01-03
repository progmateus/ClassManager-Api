using AutoMapper;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class GetTenantProfileHandler
{
  private readonly ITenantRepository _repository;
  private readonly IMapper _mapper;
  private readonly IPaymentService _paymentService;
  public GetTenantProfileHandler(
    ITenantRepository tenantRepository,
    IMapper mapper,
    IPaymentService paymentService
    )
  {
    _repository = tenantRepository;
    _mapper = mapper;
    _paymentService = paymentService;
  }
  public async Task<ICommandResult> Handle(Guid id)
  {
    var tenant = await _repository.FindAsync(x => x.Id == id, [x => x.Links, x => x.Images]);


    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    var tenantResponse = _mapper.Map<TenantViewModel>(tenant);

    var balance = _paymentService.GetBalance(tenant.StripeAccountId);

    tenantResponse.AvailableBalance = balance.Available.First().Amount;
    tenantResponse.PendingBalance = balance.Pending.First().Amount;

    if (!tenant.StripeChargesEnabled)
    {
      tenantResponse.StripeOnboardUrl = _paymentService.CreateAccountLink(tenant.StripeAccountId).Url;
    }

    return new CommandResult(true, "TENANT_GOTTEN", tenantResponse, null, 201);
  }
}
