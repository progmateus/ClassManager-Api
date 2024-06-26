using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Auth.Commands;
using ClassManager.Domain.Contexts.Auth.Services;
using ClassManager.Domain.Contexts.Shared.Enums;
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
        return new CommandResult(false, "ERR_INVALID_CREDENTIALS", null, command.Notifications);
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
        return new CommandResult(false, "ERR_INVALID_CREDENTIALS", null, null, 401);
    }
    catch (Exception)
    {
      return new CommandResult(false, "Internal server error", null, null, 500);
    }

    #endregion

    #region 03. Checa se a senha é válida

    if (!user.Password.Challenge(command.Password))
      return new CommandResult(false, "ERR_INVALID_CREDENTIALS", null, null, 401);

    #endregion

    #region 04. Checa se a conta está verificada

    try
    {
      if (user.Status != EUserStatus.ACTIVE)
        return new CommandResult(false, "ERR_USER_INACTIVE", null, null, 401);
    }
    catch
    {
      return new CommandResult(false, "Internal server error", null, null, 500);
    }

    #endregion

    #region 05. Retorna os dados
    var tokenService = new TokenService();
    var data = new AuthData
    {
      Id = user.Id.ToString(),
      Name = user.Name.ToString(),
      Email = user.Email,
      Roles = Array.Empty<string>(),
      Avatar = user.Avatar,
    };
    data.Token = tokenService.Create(data);

    var result = new CommandResult(true, "Success", data, null, 200);
    return result;

    #endregion
  }
}