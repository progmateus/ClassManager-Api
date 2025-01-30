using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Accounts.Repositories;

public class UserTokenRepository : Repository<UserToken>, IUserTokenRepository
{
  public UserTokenRepository(AppDbContext context) : base(context) { }

  public async Task<UserToken?> FindbyUserIdAndRefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken)
  {
    return await DbSet
      .AsNoTracking()
      .FirstOrDefaultAsync(x => x.UserId == userId && x.RefreshToken == refreshToken, cancellationToken);
  }
}
