using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Repositories.Contracts
{
  public interface IUserTokenRepository : IRepository<UserToken>
  {
    Task<UserToken?> FindByRefreshToken(string refreshToken, CancellationToken cancellationToken);
    Task DeleteByUserId(Guid userId, CancellationToken cancellationToken);
  }
}
