using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Handlers;
using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Data.Migrations;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Tenants.Controllers;

[Route("{tenantId}/bookings")]
public class BookingController : MainController
{
  [Authorize]
  [HttpPost()]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateBookingCommand command,
    [FromServices] CreateBookingHandler handler
  )
  {
    command.UserId = new Guid(User.FindFirst("Id")?.Value);
    var result = await handler.Handle(tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpDelete("{bookingId}")]
  public async Task<IResult> Delete(
      [FromRoute] Guid tenantId,
      [FromRoute] Guid bookingId,
      [FromBody] CreateBookingCommand command,
      [FromServices] DeleteBookingHandler handler
    )
  {
    command.UserId = new Guid(User.FindFirst("Id")?.Value);
    var result = await handler.Handle(tenantId, bookingId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

}