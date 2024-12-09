using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Handlers;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Classes.Controllers;

[Authorize]
[Route("{tenantId}/students")]
public class StudentsClasses : MainController
{
  [HttpPut("many")]
  public async Task<IResult> AddStudent(
  [FromRoute] Guid tenantId,
  [FromBody] UpdateUsersClassCommand command,
  [FromServices] UpdateStudentsClassesHandler handler
)
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut("one")]
  public async Task<IResult> AddStudent(
  [FromRoute] Guid tenantId,
  [FromBody] UpdateStudentClassCommand command,
  [FromServices] UpdateStudentClassHandler handler
)
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
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
    [FromServices] RemoveStudentFromClassHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet]
  public async Task<IResult> GetStudentclasses(
    [FromRoute] Guid tenantId,
    [FromQuery] Guid userId,
    [FromServices] ListStudentClassesHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, userId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}