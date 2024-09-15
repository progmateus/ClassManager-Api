using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class ListUsersRolesHandler
{
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IMapper _mapper;
  public ListUsersRolesHandler(
    IUsersRolesRepository userRepository,
     IMapper mapper
    )
  {
    _usersRolesRepository = userRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, List<string> rolesNames, List<Guid> usersIds)
  {
    var usersRoles = _mapper.Map<UsersRolesViewModel>(await _usersRolesRepository.ListByRoleAsync(tenantId, rolesNames, usersIds));

    return new CommandResult(true, "USERS_ROLES_LISTED", usersRoles, null, 200);
  }
}
