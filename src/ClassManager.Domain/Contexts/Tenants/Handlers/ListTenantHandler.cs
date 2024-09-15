using AutoMapper;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class ListTenantsHandler
{
  private readonly ITenantRepository _repository;
  private readonly IMapper _mapper;
  public ListTenantsHandler(
    ITenantRepository tenantRepository,
    IMapper mapper
    )
  {
    _repository = tenantRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(string search = "")
  {
    var tenants = _mapper.Map<List<TenantViewModel>>(await _repository.SearchAsync(search));

    return new CommandResult(true, "TENANTS_LISTED", tenants, null, 201);
  }
}
