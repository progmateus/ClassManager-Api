using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Classes.Controllers;

public class ClassDayController : MainController
{
  [HttpPost("class-days")]
  public async Task<IResult> Create(
    [FromBody] CreateClassDayCommand command,
    [FromServices] CreateClassDayHandler handler
  )
  {
    var result = await handler.Handle(command);
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
    var result = await handler.Handle(tenantId, classDayId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpPut("{tenantId}/class-days/{classDayId}")]
  public async Task<IResult> Update(
    [FromRoute] Guid classDayId,
    [FromBody] UpdateClassDayCommand command,
    [FromServices] UpdateClassDayHandler handler
  )
  {
    var result = await handler.Handle(classDayId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}