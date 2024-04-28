using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Tests.Mocks;
using ClassManager.Domain.Contexts.Accounts.Handlers;

namespace ClassManager.Tests.Contexts.Accounts.Handlers;

[TestClass]
public class CreateUserHandlerTests
{
    [TestMethod]
    public async Task ShouldReturnErrorWhenDocumentExists()
    {
        var handler = new CreateUserHandler(
            new FakeUserRepository(),
            new FakeEmailService()
        );

        var command = new CreateUserCommand()
        {
            FirstName = "John",
            LastName = "Doe",
            Document = "999999999",
            Email = "johnDoe@gmail.com",
            Password = "senha123",
            Avatar = null
        };

        await handler.Handle(command);
        Assert.AreEqual(false, handler.Valid);

    }
}