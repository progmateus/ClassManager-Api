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
    var result = await handler.Handle(tenantId, command);
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
    var result = await handler.Handle(timeTableId, command, tenantId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}