using AutoMapper;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class GetTenantHandler
{
  private readonly ITenantRepository _repository;
  private readonly IMapper _mapper;
  public GetTenantHandler(
    ITenantRepository tenantRepository,
    IMapper mapper
    )
  {
    _repository = tenantRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid id)
  {
    var tenant = _mapper.Map<TenantViewModel>(await _repository.GetByIdAsync(id, new CancellationToken()));

    return new CommandResult(true, "TENANT_GOTTEN", tenant, null, 201);
  }
}
