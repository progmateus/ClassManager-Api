using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Repositories.Contracts
{
  public interface IUserRepository : IRepository<User>
  {
    Task<bool> DocumentAlreadyExistsAsync(string document, CancellationToken cancellationToken);
    Task<bool> EmailAlreadyExtstsAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetRolesByIdAsync(Guid userId, CancellationToken cancellationToken);
  }
}
