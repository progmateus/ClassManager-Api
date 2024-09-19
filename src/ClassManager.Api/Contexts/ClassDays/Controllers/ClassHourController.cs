using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.ClassDays.Commands;
using ClassManager.Domain.Contexts.ClassDays.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.ClassDays.Controllers;

[Authorize]
public class ClassHourController : MainController
{
  [HttpPost("{tenantId}/classes-hours")]
  public async Task<IResult> Update(
    [FromRoute] Guid tenantId,
    [FromBody] UpdateClassHourCommand command,
    [FromServices] UpdateClassHourHandler handler
  )
  {
    var result = await handler.Handle(command, tenantId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}