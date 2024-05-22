using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Handlers;
using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Data.Migrations;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Tenants.Controllers;

[Route("{tenantId}/bookings")]
public class BookingController : MainController
{
  [HttpPost()]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid classDayId,
    [FromBody] CreateBookingCommand command,
    [FromServices] CreateBookingHandler handler
  )
  {
    var result = await handler.Handle(tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

}