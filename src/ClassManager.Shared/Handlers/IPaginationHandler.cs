using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface IPaginationHandler<T> where T : PaginationCommand
  {
    Task<ICommandResult> Handle(Guid loggedUserId, T comamnd);
  }
}
