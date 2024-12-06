using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Addresses.Controllers;

[Authorize]
public class AddressController : MainController
{
  [HttpPost("{tenantId}/addresses")]
  public async Task<IResult> Create(
    [FromRoute] Guid tenantId,
    [FromBody] CreateAddressCommand command,
    [FromServices] CreateTenantAddressHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpGet("{tenantId}/addresses")]
  public async Task<IResult> List(
    [FromRoute] Guid tenantId,
    [FromServices] ListTenantAddressesHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), tenantId);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpDelete("addresses/{id}")]
  public async Task<IResult> Delete(
    [FromRoute] Guid id,
    [FromServices] DeleteAddressHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), id);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }


  [HttpPatch("users/addresses")]
  public async Task<IResult> UpdateUserAddress(
    [FromBody] CreateAddressCommand command,
    [FromServices] UpdateUserAddressHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }

  [HttpPatch("{tenantId}/classes/{classId}/address")]
  public async Task<IResult> UpdateClassAddress(
    [FromRoute] Guid tenantId,
    [FromRoute] Guid classId,
    [FromBody] UpdateAddressCommand command,
    [FromServices] UpdateClassAddressHandler handler
  )
  {
    var result = await handler.Handle(new Guid(User.FindFirst("Id")?.Value), classId, classId, command);
    if (!result.IsSuccess)
      return Results.Json(result, statusCode: result.Status);

    if (result.Data is null)
      return Results.Json(result, statusCode: 500);

    return Results.Ok(result);
  }
}