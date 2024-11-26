using AutoMapper;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class ListUsersRolesHandler
{
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IMapper _mapper;
  private IAccessControlService _accessControlService;
  public ListUsersRolesHandler(
    IUsersRolesRepository userRepository,
    IMapper mapper,
    IAccessControlService accessControlService
    )
  {
    _usersRolesRepository = userRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, List<string> rolesNames, List<Guid> usersIds)
  {
    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }
    var usersRoles = _mapper.Map<List<UsersRolesPreviewViewModel>>(await _usersRolesRepository.ListByRoleAsync(tenantId, rolesNames, usersIds));

    return new CommandResult(true, "USERS_ROLES_LISTED", usersRoles, null, 200);
  }
}
