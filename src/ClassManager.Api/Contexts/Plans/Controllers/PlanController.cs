using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Plans.Commands;
using ClassManager.Domain.Contexts.Plans.Handlers;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using ClassManager.Domain.Shared.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Tenants.Controllers;

[Route("plans")]
public class PlanController : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
      [FromBody] PlanCommand command,
      [FromServices] CreatePlandHandler handler
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
    [FromServices] ListPlansHandler handler,
    [FromQuery] PaginationCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
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
    [FromBody] PlanCommand command,
    [FromServices] UpdatePlandHandler handler
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
    [FromServices] DeletePlanHandler handler
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