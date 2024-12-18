using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using ClassManager.Domain.Shared.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;


namespace ClassManager.Api.Contexts.Tenants.Controllers;
[Authorize]
[Route("tenants")]
public class TenantController : MainController
{
  [HttpPost]
  public async Task<IResult> Create(
      [FromBody] CreateTenantCommand command,
      [FromServices] CreateTenantHandler handler
  )
  {
    string userIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), userIp, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpGet]
  [Route("{id}")]
  public async Task<IResult> Get(
      [FromRoute] Guid id,
      [FromServices] GetTenantHandler handler
  )
  {
    var result = await handler.Handle(id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet("{id}/profile")]
  public async Task<IResult> Profile(
      [FromRoute] Guid id,
      [FromServices] GetTenantProfileHandler handler
  )
  {
    var result = await handler.Handle(id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpGet]
  public async Task<IResult> List(
    [FromServices] ListTenantsHandler handler,
    [FromQuery] PaginationCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPut]
  [Route("{id}")]
  public async Task<IResult> Update(
    [FromRoute] Guid id,
    [FromBody] UpdateTenantCommand command,
    [FromServices] UpdateTenantHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpDelete]
  [Route("{id}")]
  public async Task<IResult> Delete(
    [FromRoute] Guid id,
    [FromServices] DeleteTenantHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPost("{id}/bank-account")]
  public async Task<IResult> CreateBankAccount(
    [FromRoute] Guid id,
    [FromServices] CreateTenantBankAccountHandler handler,
    [FromBody] CreateTenantBankAccountCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpPatch("{id}/subscriptions/refresh")]
  public async Task<IResult> RefreshTenanSubscription(
    [FromRoute] Guid id,
    [FromServices] RefreshTenantSubscriptionHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpPatch("{id}/avatar")]
  public async Task<IResult> UploadAvatar(
    [FromRoute] Guid id,
    [FromServices] UploadTenantAvatarHandler handler,
    [FromBody] UploadFileCommand command
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [Authorize]
  [HttpPost("{tenantId}/images")]
  public async Task<IResult> AddImage(
    [FromRoute] Guid tenantId,
    [FromServices] CreateImageHandler handler,
    [FromBody] UploadFileCommand command
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
  [HttpDelete("{tenantId}/images/{imageId}")]
  public async Task<IResult> AddImage(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid imageId,
    [FromServices] DeleteImageHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, imageId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}