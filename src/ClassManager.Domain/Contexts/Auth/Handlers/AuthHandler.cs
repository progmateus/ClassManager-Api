using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Auth.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Contracts;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class AuthHandler :
  Notifiable,
  IHandler<AuthCommand>
{
  private readonly IUserRepository _userReporitory;
  public AuthHandler(
    IUserRepository userRepository
    )
  {
    _userReporitory = userRepository;
  }
  public async Task<ICommandResult> Handle(AuthCommand command)
  {
    // fail fast validation
    #region 01. Valida a requisição

    try
    {
      command.Validate();
      if (command.Invalid)
      {
        AddNotifications(command);
        return new CommandResult(false, "User not Created", null, command.Notifications);
      }
    }
    catch
    {
      return new CommandResult(false, "Internal server error", null, null, 500);
    }

    #endregion

    #region 02. Recupera o perfil

    User? user;
    try
    {
      user = await _userReporitory.GetByEmailAsync(command.Email, default);
      if (user is null)
        return new CommandResult(false, "Invalid Credentials", null, null, 401);
    }
    catch (Exception)
    {
      return new CommandResult(false, "Internal server error", null, null, 500);
    }

    #endregion

    #region 03. Checa se a senha é válida

    if (!user.Password.Challenge(command.Password))
      return new CommandResult(false, "Invalid Credentials", null, null, 401);

    #endregion

    #region 04. Checa se a conta está verificada

    try
    {
      if (!user.Email.Verification.IsActive)
        return new CommandResult(false, "User inactive", null, null, 401);
    }
    catch
    {
      return new CommandResult(false, "Internal server error", null, null, 500);
    }

    #endregion

    #region 05. Retorna os dados

    var data = new AuthData
    {
      Id = user.Id.ToString(),
      Name = user.Name.ToString(),
      Email = user.Email,
      Roles = Array.Empty<string>(),
      Avatar = user.Avatar,
      Token = ""
      /* Roles = user.Roles.Select(x => x.Name).ToArray() */
    };
    var result = new CommandResult(false, "Success", data, null, 500);
    result.SetTokenData(data);
    return result;

    #endregion
  }
}