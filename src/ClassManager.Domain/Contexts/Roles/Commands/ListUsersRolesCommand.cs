using ClassManager.Domain.Shared.Commands;

namespace ClasManager.Domain.Contexts.Roles.Commands;

public class ListUsersRolesCommand : PaginationCommand
{
  public List<string> RolesNames { get; set; } = [];
  public List<Guid> UsersIds { get; set; } = [];

}