using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface IPaginationHandler<T> where T : IPaginationCommand
  {
    Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, T comamnd);
  }
}
