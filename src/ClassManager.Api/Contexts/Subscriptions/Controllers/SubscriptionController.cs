using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Subscriptions.Handlers;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Subscriptions.Controllers;

[Authorize]
[Route("{tenantId}/subscriptions")]
public class TenantController : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateSubscriptionCommand command,
    [FromServices] CreateSubscriptionHandler handler
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

  [HttpGet]
  public async Task<IResult> List(
    [FromRoute] Guid tenantId,
    [FromServices] ListSubscriptionsHandler handler
  )
  {
    var result = await handler.Handle(tenantId);
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
    [FromBody] UpdateSubscriptionCommand command,
    [FromServices] UpdateSubscriptionHandler handler
  )
  {
    var result = await handler.Handle(tenantId, id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}