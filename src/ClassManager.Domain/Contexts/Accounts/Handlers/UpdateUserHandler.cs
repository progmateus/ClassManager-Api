using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class UpdateUserHandler :
  Notifiable,
  IActionHandler<UpdateUserCommand>
{
  private readonly IUserRepository _userReporitory;

  public UpdateUserHandler(
    IUserRepository userRepository
    )
  {
    _userReporitory = userRepository;
  }
  public async Task<ICommandResult> Handle(Guid id, UpdateUserCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "User not Updated", null, command.Notifications);
    }

    var user = await _userReporitory.GetByIdAsync(id, default);

    if (user is null)
    {
      return new CommandResult(false, "User not found", null, command.Notifications, 404);
    }

    // verificar se doc existe

    if ((user.Document.Number != command.Document) && await _userReporitory.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "Docuemnt already exists");
    }

    // verificar se email existe

    if ((user.Email.Address != command.Email) && await _userReporitory.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
    {
      AddNotification("Email", "E-mail already exists");
    }

    // gerar vOS
    var name = new Name(command.FirstName, command.LastName);
    var document = new Document(command.Document, EDocumentType.CPF);
    var email = new Email(command.Email);

    // gerar as entidades
    user.ChangeUser(name, email, document);

    // agrupar validações

    AddNotifications(name, document, email, user);

    if (Invalid)
    {
      return new CommandResult(false, "User not created", null, Notifications);
    }

    // salvar as informações

    await _userReporitory.UpdateAsync(user, default);

    // retornar infos

    return new CommandResult(true, "User created", user, null, 201);
  }
}
