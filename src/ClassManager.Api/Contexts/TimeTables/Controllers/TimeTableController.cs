using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.TimeTables.Commands;
using ClassManager.Domain.Contexts.TimeTables.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.ClassDays.Controllers;

[Authorize]
[Route("{tenantId}/time-tables")]
public class TimeTableController : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateTimeTableCommand command,
    [FromServices] CreateTimeTableHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet]
  public async Task<IResult> List(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid timeTableId,
    [FromServices] ListTimesTablesHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{timeTableId}")]
  public async Task<IResult> Get(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid timeTableId,
    [FromServices] GetTimeTableHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, timeTableId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut("{timeTableId}")]
  public async Task<IResult> Update(
  [FromRoute] Guid tenantId,
  [FromRoute] Guid timeTableId,
  [FromBody] UpdateTimeTableCommand command,
  [FromServices] UpdateTimetableHandler handler
)
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, timeTableId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}