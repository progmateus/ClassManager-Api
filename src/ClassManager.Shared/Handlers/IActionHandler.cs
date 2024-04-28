using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface IActionHandler<T> where T : ICommand
  {
    Task<ICommandResult> Handle(Guid id, T comamnd);
  }
}
