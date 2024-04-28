using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface IHandler<T> where T : ICommand
  {
    Task<ICommandResult> Handle(T comamnd);
  }
}
