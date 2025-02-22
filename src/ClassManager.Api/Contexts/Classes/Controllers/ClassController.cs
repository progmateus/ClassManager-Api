using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Handlers;
using ClassManager.Domain.Shared.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Classes.Controllers;

[Authorize]
[Route("{tenantId}/classes")]
public class ClassController : MainController
{

  [HttpPost]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] ClassCommand command,
    [FromServices] CreateClassHandler handler
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
    [FromServices] ListClassesHandler handler,
    [FromQuery] PaginationCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{id}")]
  public async Task<IResult> GetById(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid id,
    [FromServices] GetClassByIdHandler handler
  )
  {
    var result = await handler.Handle(tenantId, id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{id}/profile")]
  public async Task<IResult> Profile(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid id,
    [FromServices] GetClassProfileHandler handler
  )
  {
    var result = await handler.Handle(tenantId, id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{id}/students")]
  public async Task<IResult> ListStudents(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid id,
    [FromServices] ListStudentsByClassHandler handler,
    [FromQuery] PaginationCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{id}/teachers")]
  public async Task<IResult> ListTeachers(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid id,
    [FromServices] ListTeachersByClassHandler handler,
    [FromQuery] PaginationCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPatch]
  [Route("{id}/students/transfer")]
  public async Task<IResult> TransferStudents(
    [FromRoute] Guid id,
  [FromRoute] Guid tenantId,
  [FromBody] TransferClassStudentsCommand command,
  [FromServices] TransferClassStudentsHandler handler
)
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut]
  [Route("{id}")]
  public async Task<IResult> Update(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid id,
    [FromBody] ClassCommand command,
    [FromServices] UpdateClassHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id, command);
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
    [FromServices] DeleteClassHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut]
  [Route("{id}/time-table")]
  public async Task<IResult> UpdateTimeTable(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid id,
    [FromBody] UpdateClassTimeTableCommand command,
    [FromServices] UpdateClassTimeTableHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}