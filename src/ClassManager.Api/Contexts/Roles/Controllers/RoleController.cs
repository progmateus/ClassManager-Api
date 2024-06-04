using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Roles.Controllers;
[Authorize]
[Route("roles")]
public class Rolecontroller : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
      [FromBody] RoleCommand command,
      [FromServices] CreateRoleHandler handler
  )
  {
    var result = await handler.Handle(command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet]
  public async Task<IResult> List(
    [FromServices] ListRolesHandler handler
  )
  {
    var result = await handler.Handle();
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut]
  [Route("{id}")]
  public async Task<IResult> Update(
    [FromRoute] Guid id,
    [FromBody] RoleCommand command,
    [FromServices] UpdateRoleHandler handler
  )
  {
    var result = await handler.Handle(id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpDelete]
  [Route("{id}")]
  public async Task<IResult> Delete(
    [FromRoute] Guid id,
    [FromServices] DeleteroleHandler handler
  )
  {
    var result = await handler.Handle(id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}