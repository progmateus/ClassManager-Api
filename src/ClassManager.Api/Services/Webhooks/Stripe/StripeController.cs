using ClasManager.Domain.Contexts.Bookings.Commands;
using ClasManager.Domain.Contexts.Bookings.Handlers;
using ClassManager.Api.Contexts.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ClassManager.Api.Contexts.Tenants.Controllers;

[Route("webhooks/stripe")]
public class StripeController : MainController
{
  [Authorize]
  [HttpPost]
  public async Task<IResult> Create(
    [FromBody] Event stripeEvent,
    [FromServices] CreateBookingHandler handler
  )
  {
    try
    {
      Console.WriteLine("=====================================");
      Console.WriteLine("=====================================");
      Console.WriteLine("=====================================");
      Console.WriteLine("=====================================");
      Console.WriteLine("=====================================");
      Console.WriteLine(stripeEvent.Type);
      // Handle the event
      // If on SDK version < 46, use class Events instead of EventTypes
      if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
      {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        // Then define and call a method to handle the successful payment intent.
        // handlePaymentIntentSucceeded(paymentIntent);
      }
      else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
      {
        var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
        // Then define and call a method to handle the successful attachment of a PaymentMethod.
        // handlePaymentMethodAttached(paymentMethod);
      }
      // ... handle other event types
      else
      {
        // Unexpected event type
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
}