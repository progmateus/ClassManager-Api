using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Accounts.Controllers;

[Route("users")]
public class UserController : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
      [FromBody] CreateUserCommand command,
      [FromServices] CreateUserHandler handler
  )
  {
    var result = await handler.Handle(command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpGet]
  public async Task<IResult> List(
    [FromServices] ListUsersHandler handler
  )
  {
    var result = await handler.Handle();
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
  [Authorize]
  [HttpPut]
  public async Task<IResult> Update(
    [FromBody] UpdateUserCommand command,
    [FromServices] UpdateUserHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpDelete]
  public async Task<IResult> Delete(
    [FromRoute] Guid id,
    [FromServices] DeleteUserHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value));
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [Authorize]
  [HttpGet("profile")]
  public async Task<IResult> Profile(
    [FromServices] GetUserProfileHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value));
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpGet("username")]
  public async Task<IResult> GetByUsername(
    [FromServices] GetUserByUsernameHandler handler,
    [FromQuery] string username
  )
  {
    var result = await handler.Handle(username);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    /* if (result.Data is null)
      return Results.Json(result, statusCode: 500); */

    return Results.Ok(result);
  }
}