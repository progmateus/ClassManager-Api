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

    if ((user.Document.Number != command.Document) && await _userReporitory.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "Document already exists");
    }

    // verificar se email existe

    if ((user.Email.Address != command.Email) && await _userReporitory.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
    {
      AddNotification("Email", "E-mail already exists");
    }

    // gerar vOS
    var document = new Document(command.Document);
    var email = new Email(command.Email);
    var phone = new Phone(command.Phone);


    AddNotifications(document, email, phone);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, Notifications);
    }

    user.ChangeUser(command.Name, email, document, phone);

    await _userReporitory.UpdateAsync(user, default);

    return new CommandResult(true, "USER_UPDATED", _mapper.Map<UserViewModel>(user), null, 201);
  }
}
