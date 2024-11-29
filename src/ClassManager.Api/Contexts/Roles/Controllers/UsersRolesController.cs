using ClasManager.Domain.Contexts.Roles.Commands;
using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Roles.Controllers;
[Authorize]
[Route("{tenantId}")]
public class UsersrolesController : MainController
{

  [HttpPost("users-roles")]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateUserRoleCommand command,
    [FromServices] CreateUserRoleHandler handler
)
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut("users-roles")]
  public async Task<IResult> Update(
    [FromRoute] Guid tenantId,
      [FromBody] UsersRolesCommand command,
      [FromServices] UpdateUsersRolesHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpGet("users-roles")]
  public async Task<IResult> ListUsersRoles(
      [FromRoute] Guid tenantId,
      [FromQuery] ListUsersRolesCommand command,
      [FromServices] ListUsersRolesHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpDelete("users-roles/{id}")]
  public async Task<IResult> Delete(
      [FromRoute] Guid tenantId,
      [FromRoute] Guid id,
      [FromServices] DeleteUserRoleHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}