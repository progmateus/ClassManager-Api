using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Repositories.Contracts
{
  public interface IUserRepository : IRepository<User>
  {
    Task<bool> DocumentAlreadyExistsAsync(string document, CancellationToken cancellationToken);
    Task<bool> EmailAlreadyExtstsAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> VerifyUserRoleAsync(Guid userId, Guid tenantId, string roleName, CancellationToken cancellationToken);
    Task<User?> GetByIdWithIncludeAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetByUsername(string username, CancellationToken cancellationToken);
  }
}
