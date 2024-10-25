using ClassManager.Domain;
using ClassManager.Domain.Services;
using Stripe;

namespace ClassManager.Data.Contexts.Tenants.Services;

public class PaymentService : IPaymentService
{

  public PaymentService()
  {
    StripeConfiguration.ApiKey = Configuration.Stripe.ApiKey;
  }

  public Account CreateAccount(Guid tenantId, string tenantEmail)
  {
    var options = new AccountCreateOptions
    {
      Country = "BR",
      Email = tenantEmail,
      Controller = new AccountControllerOptions
      {
        Fees = new AccountControllerFeesOptions { Payer = "application" },
        Losses = new AccountControllerLossesOptions { Payments = "application" },
        StripeDashboard = new AccountControllerStripeDashboardOptions
        {
          Type = "express",
        },
      },
    };
    var service = new AccountService();
    return service.Create(options);
  }

  public Customer CreateCustomer(string name, string email, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };
    var options = new CustomerCreateOptions
    {
      Name = name,
      Email = email,
    };

    var service = new CustomerService();
    return service.Create(options, requestOptions);
  }

  public Invoice CreateInvoice(Guid tenantId, string stripeCustomerId, string stripeSubscriptionId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };
    var options = new InvoiceCreateOptions
    {
      Customer = stripeCustomerId,
      Metadata = new Dictionary<string, string>
      {
        { "tenantId", tenantId.ToString() },
      },
      PaymentSettings = new InvoicePaymentSettingsOptions
      {
        PaymentMethodTypes = ["card", "boleto", "pix"]
      }
    };
    var service = new InvoiceService();
    return service.Create(options, requestOptions);
  }

  public Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new PriceCreateOptions
    {
      Currency = "brl",
      UnitAmount = Convert.ToInt64(priceInCents),
      Recurring = new PriceRecurringOptions { Interval = "month" },
      Product = stripeProductId,
      Metadata = new Dictionary<string, string>
      {
        { "tenantId", tenantId.ToString() ?? "" },
        { "databaseId", productEntityId.ToString() }
      }
    };
    var service = new PriceService();
    return service.Create(options, requestOptions);
  }

  public Product CreateProduct(Guid entityId, string ownerType, string name, Guid? tenantId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new ProductCreateOptions
    {
      Name = name,
      Metadata = new Dictionary<string, string>
      {
        { "tenantId", tenantId.ToString() ?? "" },
        { "ownerType", ownerType },
        { "databaseId", entityId.ToString() }
      }
    };
    var service = new ProductService();
    return service.Create(options, requestOptions);
  }

  public Subscription CreateSubscription(Guid tenantId, string stripePriceId, string stripeCustomerId, string? connectedAccountId)
  {

    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new SubscriptionCreateOptions
    {
      Customer = stripeCustomerId,
      Currency = "brl",
      OffSession = true,
      PaymentBehavior = "default_incomplete",
      Items = new List<SubscriptionItemOptions>
      {
          new SubscriptionItemOptions { Price = stripePriceId },
      },
      Metadata = new Dictionary<string, string>
      {
        { "tenantId", tenantId.ToString() ?? "" }
      }
    };
    var service = new SubscriptionService();
    return service.Create(options, requestOptions);
  }

  public void RequestUsingConnectedAccount()
  {
    var options = new PaymentIntentCreateOptions
    {
      Amount = 1000,
      Currency = "usd",
      AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
      {
        Enabled = true,
      },
    };
    var requestOptions = new RequestOptions
    {
      StripeAccount = "{{CONNECTED_ACCOUNT_ID}}",
    };
    var service = new PaymentIntentService();
    service.Create(options, requestOptions);
  }
}
