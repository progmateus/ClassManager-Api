using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Handlers;
using ClasManager.Domain.Contexts.ClassDays.Handlers;
using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.ClassDays.Commands;
using ClassManager.Domain.Contexts.ClassDays.Handlers;
using ClassManager.Domain.Contexts.Classes.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.ClassDays.Controllers;

[Authorize]
public class ClassDayController : MainController
{
  [HttpPost("{tenantId}/class-days")]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateClassDayCommand command,
    [FromServices] CreateClassDayHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{tenantId}/class-days/{classDayId}")]
  public async Task<IResult> Get(
    [FromRoute] Guid classDayId,
    [FromRoute] Guid tenantId,
    [FromServices] GetClassDayByIdHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, classDayId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{tenantId}/class-days/{classDayId}/bookings")]
  public async Task<IResult> GetBookings(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid classDayId,
    [FromQuery] ListBookingsCommand command,
    [FromServices] ListClassDayBookingsHandler handler
  )
  {
    command.ClassDayId = classDayId;
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpGet("class-days")]
  public async Task<IResult> List(
    [FromServices] ListClassesDaysHandler handler,
    [FromQuery] ListClassesDaysCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpPut("{tenantId}/class-days/{classDayId}")]
  public async Task<IResult> Update(
    [FromRoute] Guid classDayId,
    [FromRoute] Guid tenantId,
    [FromBody] UpdateClassDayCommand command,
    [FromServices] UpdateClassDayHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, classDayId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpDelete("{tenantId}/class-days/{classDayId}")]
  public async Task<IResult> Delete(
    [FromRoute] Guid classDayId,
    [FromRoute] Guid tenantId,
    [FromServices] DeleteClassDayHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, classDayId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpPost("class-days/generate")]
  public async Task<IResult> Generate(
    [FromServices] GenerateClassesDaysHandler handler
  )
  {
    var result = await handler.Handle();
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}