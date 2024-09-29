using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Tenants.Controllers;
[Authorize]
[Route("tenants")]
public class TenantController : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
      [FromBody] CreateTenantCommand command,
      [FromServices] CreateTenantHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpGet]
  [Route("{id}")]
  public async Task<IResult> Get(
      [FromRoute] Guid id,
      [FromServices] GetTenantHandler handler
  )
  {
    var result = await handler.Handle(id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet]
  public async Task<IResult> List(
    [FromServices] ListTenantsHandler handler,
    [FromQuery] string? search
  )
  {
    var result = await handler.Handle(search);
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
    [FromBody] UpdateTenantCommand command,
    [FromServices] UpdateTenantHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id, command);
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
    [FromServices] DeleteTenantHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}