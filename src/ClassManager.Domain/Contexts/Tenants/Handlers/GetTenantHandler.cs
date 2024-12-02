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
    var tenant = _mapper.Map<TenantViewModel>(await _repository.FindAsync(x => x.Id == id, [x => x.Links]));

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    return new CommandResult(true, "TENANT_GOTTEN", tenant, null, 201);
  }
}
