using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Classes.Controllers;

[Route("class-days")]
public class ClassDayController : MainController
{
  [HttpPost]
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


  [HttpPut("{classDayId}")]
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