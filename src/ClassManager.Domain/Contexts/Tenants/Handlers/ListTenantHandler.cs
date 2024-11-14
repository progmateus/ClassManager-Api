using AutoMapper;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class ListTenantsHandler
{
  private readonly ITenantRepository _tenantsRepository;
  private readonly IMapper _mapper;
  public ListTenantsHandler(
    ITenantRepository tenantRepository,
    IMapper mapper
    )
  {
    _tenantsRepository = tenantRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(string search = "", int page = 1)
  {
    if (page < 1) page = 1;

    var limit = 30;

    var skip = (page - 1) * limit;

    var tenants = _mapper.Map<List<TenantViewModel>>(await _tenantsRepository.SearchAsync(skip, limit, search));

    return new CommandResult(true, "TENANTS_LISTED", tenants, null, 201);
  }
}
