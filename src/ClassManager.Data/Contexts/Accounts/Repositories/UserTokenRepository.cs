using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Accounts.Repositories;

public class UserTokenRepository : Repository<UserToken>, IUserTokenRepository
{
  public UserTokenRepository(AppDbContext context) : base(context) { }

  public async Task<UserToken?> FindByRefreshToken(string refreshToken, CancellationToken cancellationToken)
  {
    return await DbSet
      .AsNoTracking()
      .Include(x => x.User)
      .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);
  }
}
