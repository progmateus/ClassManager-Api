using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Users.ViewModels;
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
  private readonly IMapper _mapper;

  public UpdateUserHandler(
    IUserRepository userRepository,
    IMapper mapper
    )
  {
    _userReporitory = userRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid id, UpdateUserCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var user = await _userReporitory.GetByIdAsync(id, default);

    if (user is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, command.Notifications, 404);
    }

    // verificar se doc existe

    if ((user.Document.Number != command.Document) && await _userReporitory.DocumentAlreadyExistsAsync(command.Document.Replace(".", "").Replace("-", "").Replace(" ", ""), new CancellationToken()))
    {
      AddNotification("Document", "Document already exists");
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
    user.ChangeUser(name, email, document, command.Phone);

    // agrupar validações

    AddNotifications(name, document, email, user);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, Notifications);
    }

    // salvar as informações

    await _userReporitory.UpdateAsync(user, default);

    // retornar infos

    return new CommandResult(true, "USER_UPDATED", _mapper.Map<UserPreviewViewModel>(user), null, 201);
  }
}
