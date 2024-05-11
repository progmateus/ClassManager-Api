using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Handlers;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Classes.Controllers;

[Route("{tenantId}/students")]
public class StudentsClasses : MainController
{

  [HttpPost]
  public async Task<IResult> AddTeacher(
  [FromRoute] Guid tenantId,
  [FromBody] CreateUserClassCommand command,
  [FromServices] AddStudentsClassesHandler handler
)
  {
    var result = await handler.Handle(tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpDelete]
  [Route("{id}")]
  public async Task<IResult> Delete(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid id,
    [FromServices] RemoveStudentsClassesHandler handler
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