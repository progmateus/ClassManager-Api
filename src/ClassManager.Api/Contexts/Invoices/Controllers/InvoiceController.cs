using ClasManager.Domain.Contexts.Invoices.Commands;
using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Invoices.Handlers;
using ClassManager.Domain.Shared.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.ClassDays.Controllers;

[Authorize]
public class InvoiceController : MainController
{
  [HttpPost("{tenantId}/invoices")]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateInvoiceCommand command,
    [FromServices] CreateInvoiceHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("invoices")]
  public async Task<IResult> List(
  [FromServices] ListInvoicesHandler handler,
  [FromQuery] ListInvoicesCommand command
)
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPatch("{tenantId}/invoices{invoiceId}")]
  public async Task<IResult> Update(
  [FromRoute] Guid tenantId,
  [FromRoute] Guid invoiceId,
  [FromServices] PayInvoiceHandler handler
)
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, invoiceId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}