using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Entities
{
  public class UserToken : Entity
  {

    protected UserToken()
    {

    }

    public UserToken(Guid userId, string refreshToken, DateTime expiresAt)
    {
      UserId = userId;
      RefreshToken = refreshToken;
      ExpiresAt = expiresAt;
    }

    public string RefreshToken { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public DateTime ExpiresAt { get; private set; }
  }
}