using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Roles.Controllers;

[Route("users-roles")]
public class UsersrolesController : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
      [FromBody] UsersRolesCommand command,
      [FromServices] UpdateUsersRolesHandler handler
  )
  {
    var result = await handler.Handle(command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}