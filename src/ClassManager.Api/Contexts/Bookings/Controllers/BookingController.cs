using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Handlers;
using ClassManager.Api.Contexts.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Tenants.Controllers;

public class BookingController : MainController
{
  [Authorize]
  [HttpPost("{tenantId}/bookings")]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateBookingCommand command,
    [FromServices] CreateBookingHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpGet("/bookings")]
  public async Task<IResult> List(
    [FromServices] ListBookingsHandler handler,
    [FromQuery] ListBookingsCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpDelete("{tenantId}/bookings/{bookingId}")]
  public async Task<IResult> Delete(
      [FromRoute] Guid tenantId,
      [FromRoute] Guid bookingId,
      [FromServices] DeleteBookingHandler handler
    )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, bookingId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

}