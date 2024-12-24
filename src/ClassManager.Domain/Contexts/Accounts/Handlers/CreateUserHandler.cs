using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Users.ViewModels;
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
  private readonly IMapper _mapper;

  public CreateUserHandler(
    IUserRepository userRepository,
    IEmailService emailService,
    IMapper mapper
    )
  {
    _userReporitory = userRepository;
    _emailService = emailService;
    _mapper = mapper;
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

    // verificar se o nome de usuário existe

    if (await _userReporitory.UsernameAlreadyExistsAsync(command.Username, new CancellationToken()))
    {
      AddNotification("Username", "ERR_USERNAME_ALREADY_EXISTS");
    }

    // gerar vOS
    var document = new Document(command.Document);
    var email = new Email(command.Email);
    var password = new Password(command.Password);

    // gerar as entidades
    var user = new User(command.Name, document, email, password, command.Username, command.Phone, command.Avatar);

    // agrupar validações

    AddNotifications(document, email, user);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, Notifications);
    }

    // salvar as informações

    await _userReporitory.CreateAsync(user, new CancellationToken());

    // enviar email de boas vindas 
    await _emailService.SendVerificationEmailAsync(user, new CancellationToken());
    // retornar infos

    return new CommandResult(true, "USER_CREATED", _mapper.Map<UserViewModel>(user), null, 201);
  }
}
