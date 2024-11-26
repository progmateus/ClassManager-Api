using AutoMapper;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class ListTenantsHandler : IPaginationHandler<PaginationCommand>
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
  public async Task<ICommandResult> Handle(Guid loggedUserId, PaginationCommand command)
  {
    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var tenants = _mapper.Map<List<TenantPreviewViewModel>>(await _tenantsRepository.SearchAsync(skip, command.Limit, command.Search));

    return new CommandResult(true, "TENANTS_LISTED", tenants, null, 201);
  }
}
