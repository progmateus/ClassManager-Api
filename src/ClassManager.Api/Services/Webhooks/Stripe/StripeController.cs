using ClassManager.Api.Contexts.Shared.Controllers;
using ClassManager.Domain.Services.Stripe.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ClassManager.Api.Contexts.Tenants.Controllers;

[Route("webhooks/stripe")]
public class StripeController : MainController
{
  [HttpPost("listen")]
  public async Task<IResult> Listen(
    [FromServices] FinalizeStripeInvoiceWebhookHandler finalizeStripeInvoiceWebhookHandler,
    [FromServices] CreateStripeSubscriptionWebhookHandler createStripeSubscriptionWebhookHandler,
    [FromServices] UpdateStripeAccountWebhookHandler updateStripeAccountWebhookHandler,
    [FromServices] UpdateStripeInvoiceWebhookHandler updateStripeInvoiceWebhookHandler,
    [FromServices] UpdateStripeSubscriptionWebhookHandler updateStripeSubscriptionWebhookHandler,
    [FromServices] CreateStripePayoutWebhookHandler createStripeWebhookHandler,
    [FromServices] UpdateStripePayoutWebhookHandler updateStripePayoutWebhookHandler,
    [FromServices] CreateStripeExternalBankAccountWebhookHandler createStripeExternalBankAccountWebhookHandler,
    [FromServices] UpdateStripeExternalBankAccountWebhookHandler updateStripeExternalBankAccountWebhookHandler,
    [FromServices] CreateStripePaymentWebhookHandler createStripePaymentWebhookHandler
  )
  {
    try
    {
      var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

      var stripeEvent = EventUtility.ParseEvent(json);

      if (stripeEvent.Type == EventTypes.InvoiceFinalized)
      {
        await finalizeStripeInvoiceWebhookHandler.Handle(stripeEvent.Data.Object as Invoice);
      }
      else if (stripeEvent.Type == EventTypes.InvoiceUpdated)
      {
        await updateStripeInvoiceWebhookHandler.Handle(stripeEvent.Data.Object as Invoice);
      }
      else if (stripeEvent.Type == EventTypes.InvoicePaymentSucceeded)
      {
        await createStripePaymentWebhookHandler.Handle(stripeEvent.Data.Object as Invoice);
      }
      else if (
        stripeEvent.Type == EventTypes.CustomerSubscriptionCreated)
      {
        await createStripeSubscriptionWebhookHandler.Handle(stripeEvent.Data.Object as Subscription);
      }
      else if (
        stripeEvent.Type == EventTypes.CustomerSubscriptionPaused ||
        stripeEvent.Type == EventTypes.CustomerSubscriptionDeleted ||
        stripeEvent.Type == EventTypes.CustomerSubscriptionResumed ||
        stripeEvent.Type == EventTypes.CustomerSubscriptionUpdated)
      {
        await updateStripeSubscriptionWebhookHandler.Handle(stripeEvent.Data.Object as Subscription);
      }
      else if (stripeEvent.Type == EventTypes.AccountUpdated)
      {
        await updateStripeAccountWebhookHandler.Handle(stripeEvent.Data.Object as Account);
      }
      else if (stripeEvent.Type == EventTypes.PayoutCreated)
      {
        await createStripeWebhookHandler.Handle(stripeEvent.Data.Object as Payout);
      }
      else if (
        stripeEvent.Type == EventTypes.PayoutUpdated
        )
      {
        await updateStripePayoutWebhookHandler.Handle(stripeEvent.Data.Object as Payout);
      }
      else if (
        stripeEvent.Type == EventTypes.AccountExternalAccountCreated
        )
      {
        await createStripeExternalBankAccountWebhookHandler.Handle(stripeEvent.Data.Object as BankAccount);
      }
      else if (
        stripeEvent.Type == EventTypes.AccountExternalAccountUpdated
        )
      {
        await updateStripeExternalBankAccountWebhookHandler.Handle(stripeEvent.Data.Object as BankAccount);
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