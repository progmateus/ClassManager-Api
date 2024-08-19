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

public class CreateUserHandler :
  Notifiable,
  IHandler<CreateUserCommand>
{
  private readonly IUserRepository _userReporitory;
  private readonly IEmailService _emailService;

  public CreateUserHandler(
    IUserRepository userRepository,
    IEmailService emailService
    )
  {
    _userReporitory = userRepository;
    _emailService = emailService;
  }
  public async Task<ICommandResult> Handle(CreateUserCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    // verificar se doc existe

    if (await _userReporitory.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "ERR_DOCUMENT_ALREADY_EXISTS");
    }

    // verificar se email existe

    if (await _userReporitory.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
    {
      AddNotification("Email", "ERR_EMAIL_ALREADY_EXISTS");
    }

    // gerar vOS
    var name = new Name(command.FirstName, command.LastName);
    var document = new Document(command.Document, EDocumentType.CPF);
    var email = new Email(command.Email);
    var password = new Password(command.Password);

    // gerar as entidades
    var user = new User(name, document, email, password, command.Avatar);

    // agrupar validações

    AddNotifications(name, document, email, user);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_USER_NOT_CREATED", null, Notifications);
    }

    // salvar as informações

    await _userReporitory.CreateAsync(user, new CancellationToken());

    // enviar email de boas vindas 
    await _emailService.SendVerificationEmailAsync(user, new CancellationToken());
    // retornar infos

    return new CommandResult(true, "USER_CREATED", user, null, 201);
  }
}
