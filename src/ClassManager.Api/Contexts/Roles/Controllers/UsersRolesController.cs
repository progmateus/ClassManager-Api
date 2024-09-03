using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Roles.Controllers;
[Authorize]
public class UsersrolesController : MainController
{

  [HttpPost("{tenantId}/users-roles")]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateUserRoleCommand command,
    [FromBody] string roleName,
    [FromServices] CreateUserRoleHandler handler
)
  {
    var result = await handler.Handle(command, tenantId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut("users-roles")]
  public async Task<IResult> Update(
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


  [HttpGet("{tenantId}/users-roles")]
  public async Task<IResult> ListUsersRoles(
      [FromRoute] Guid tenantId,
      [FromQuery] List<string> rolesNames,
      [FromQuery] List<Guid> usersIds,
      [FromServices] ListUsersRolesHandler handler
  )
  {
    var result = await handler.Handle(tenantId, rolesNames, usersIds);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpDelete("{tenantId}/users-roles/{id}")]
  public async Task<IResult> Delete(
      [FromRoute] Guid tenantId,
      [FromRoute] Guid id,
      [FromServices] DeleteUserRoleHandler handler
  )
  {
    var result = await handler.Handle(tenantId, id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}