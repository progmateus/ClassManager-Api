using System.Linq.Expressions;
using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Queries
{
  public static class UserQueries
  {

    public static Expression<Func<User, bool>> GetUserInfo(string document)
    {
      return x => x.Document.Number == document;
    }
  }
}
