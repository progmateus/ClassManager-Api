using AutoMapper;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
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
    var tenant = await _repository.FindAsync(x => x.Id == id, [x => x.Links]);

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }


    _paymentService.CreateVerificationSession(tenant.Email, tenant.StripeAccountId);

    return new CommandResult(true, "TENANT_GOTTEN", tenant, null, 201);
  }
}
