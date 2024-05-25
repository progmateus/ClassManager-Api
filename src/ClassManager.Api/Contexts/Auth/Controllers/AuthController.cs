using System.Reflection;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Auth.Commands;
using ClassManager.Domain.Contexts.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Auth.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : ControllerBase
{
  [HttpPost("login")]
  public async Task<IResult> Create(
      [FromBody] AuthCommand command,
      [FromServices] TokenService tokenService,
      [FromServices] AuthHandler handler
  )
  {
    var result = await handler.Handle(command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    return Results.Ok(result);
  }
}