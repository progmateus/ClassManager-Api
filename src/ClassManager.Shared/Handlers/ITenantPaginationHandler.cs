using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface ITenantPaginationHandler<T> where T : PaginationCommand
  {
    Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, T comamnd);
  }
}
