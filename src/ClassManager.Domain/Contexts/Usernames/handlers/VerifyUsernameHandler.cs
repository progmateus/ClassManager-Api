using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Usernames.Handlers;

public class VerifyUsernameHandler :
  Notifiable,
  IHandler<VerifyUsernameCommand>
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IUserRepository _usersRepository;

  public VerifyUsernameHandler(
    ITenantRepository tenantRepository,
    IUserRepository usersRepository
    )
  {
    _tenantRepository = tenantRepository;
    _usersRepository = usersRepository;
  }
  public async Task<ICommandResult> Handle(VerifyUsernameCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_INVALID_USERNAME", null, command.Notifications, 400);
    }


    if (await _tenantRepository.UsernameAlreadyExistsAsync(command.Username, new CancellationToken()))
    {
      return new CommandResult(true, "ERR_USERNAME_ALREADY_EXISTS", "", null, 200);
    }

    if (await _usersRepository.UsernameAlreadyExistsAsync(command.Username, new CancellationToken()))
    {
      return new CommandResult(true, "ERR_USERNAME_ALREADY_EXISTS", "", null, 200);
    }


    return new CommandResult(true, "USERNAME_AVAILABLE", "", null, 200);
  }
}
