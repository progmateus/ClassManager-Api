using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Repositories.Contracts
{
  public interface IUserTokenRepository : IRepository<UserToken>
  {
    Task<UserToken?> FindByrefreshToken(string refreshToken, CancellationToken cancellationToken);
  }
}
