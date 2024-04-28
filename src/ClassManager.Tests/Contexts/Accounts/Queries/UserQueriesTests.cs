using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Accounts.Queries;

namespace ClassManager.Tests.Contexts.Accounts.Queries;

[TestClass]
public class UserQueriesTests
{
  private IList<User> _users;

  public UserQueriesTests()
  {
    _users = new List<User>();
    for (var i = 0; i <= 10; i++)
    {
      _users.Add(new User(
          new Name("Aluno", i.ToString()),
          new Document("1111111111" + i.ToString(), EDocumentType.CPF),
          new Email(i.ToString() + "@balta.io"),
          new Password("senha1234"),
          null
      ));
    }
  }
  [TestMethod]
  public void ShouldReturnNullWheDocumentNotExists()
  {
    var exp = UserQueries.GetUserInfo("999999999");
    var stdn = _users.AsQueryable().Where(exp).FirstOrDefault();
    Assert.AreEqual(null, stdn);

  }

  [TestMethod]
  public void ShouldReturnStudentWhenDocumentExists()
  {
    var exp = UserQueries.GetUserInfo("11111111111");
    var studn = _users.AsQueryable().Where(exp).FirstOrDefault();

    Assert.AreNotEqual(null, studn);
  }
}