using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface ITenantHandler<T> where T : ICommand
  {
    Task<ICommandResult> Handle(Guid tenantId, Guid loggedUserId, T comamnd);
  }
}
