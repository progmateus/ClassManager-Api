using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Handlers;
using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Services.Stripe.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;

namespace ClassManager.Api.Contexts.Tenants.Controllers;

[Route("webhooks/stripe")]
public class StripeController : MainController
{
  [HttpPost("listen")]
  public async Task<IResult> Listen(
    [FromServices] UpdateInvoiceStripeWebhookHandler updateInvoiceStripeWebhookHandler
  )
  {
    try
    {
      var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

      var stripeEvent = EventUtility.ParseEvent(json);

      if (stripeEvent.Type == EventTypes.InvoiceFinalized)
      {
        await updateInvoiceStripeWebhookHandler.Handle(stripeEvent.Data.Object as Invoice);
      }

      if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
      {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

      }
      else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
      {
        var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
      }
      else
      {
        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
      }
      return Results.Ok();
    }
    catch (StripeException e)
    {
      return Results.BadRequest();
    }

    return Results.Ok();
  }


  [Authorize]
  [HttpPost("create")]
  public IResult Create(
  [FromServices] CreateStripeWebhookHandler handler
)
  {
    handler.Handle();
    return Results.Ok();
  }
}