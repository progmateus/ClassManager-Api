using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Tests.Contexts.Accounts.Entities;

[TestClass]
public class UserTests
{
    [TestMethod]
    [TestCategory("UserTests")]
    public void ShouldReturnSuccesseWhenCreateAVali()
    {
        var name = new Name("John", "Doe");
        var document = new Document("86381039051", EDocumentType.CPF);
        var email = new Email("johndoe@gmail.com");
        var user = new User(name, document, email, new Password("senha123"), null);
        Assert.IsTrue(user.Valid);
    }

    [TestMethod]
    [TestCategory("UserTests")]
    public void ShouldReturnErrorWhenUserIsInvalid()
    {
        var name = new Name("", "");
        var document = new Document("86381039051", EDocumentType.CPF);
        var email = new Email("johndoe@gmail.com");
        var user = new User(name, document, email, new Password("senha123"), null);
        Assert.IsTrue(user.Invalid);
    }

    [TestMethod]
    [TestCategory("UserTests")]
    public void ShouldReturnSuccessWhenChangeUserName()
    {
        var name = new Name("John", "Doe");
        var document = new Document("86381039051", EDocumentType.CPF);
        var email = new Email("johndoe@gmail.com");
        var user = new User(name, document, email, new Password("senha123"), null);

        var newName = new Name("Jane", "Doe");

        user.ChangeUser(newName, user.Email, user.Document);
        Assert.AreEqual(newName, user.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    [TestCategory("UserTests")]
    public void ShouldReturnExceptionWhenEmailChangedIsNull()
    {
        var name = new Name("John", "Doe");
        var document = new Document("86381039051", EDocumentType.CPF);
        var email = new Email("johndoe@gmail.com");
        var user = new User(name, document, email, new Password("senha123"), null);
        user.ChangeUser(user.Name, null, user.Document);
    }
}