using ClassManager.Domain;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Services;
using Stripe;

namespace ClassManager.Data.Contexts.Tenants.Services;

public class StripeService : IStripeService
{

  public StripeService()
  {
    StripeConfiguration.ApiKey = Configuration.Stripe.ApiKey;
  }
  public Customer CreateCustomer(Tenant tenant)
  {
    var options = new CustomerCreateOptions
    {
      Name = tenant.Name,
      Email = tenant.Email,
    };

    var service = new CustomerService();
    return service.Create(options);
  }

  public Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents)
  {

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
    return service.Create(options);
  }

  public Product CreateProduct(Guid entityId, string ownerType, string name, Guid? tenantId)
  {
    var options = new ProductCreateOptions
    {
      Id = entityId.ToString(),
      Name = name,
      Metadata = new Dictionary<string, string>
      {
        { "tenantId", tenantId.ToString() ?? "" },
        { "ownerType", ownerType }
      }
    };
    var service = new ProductService();
    return service.Create(options);
  }

  public Subscription CreateSubscription(Guid tenantId, string stripePriceId, string stripeCustomerId)
  {
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
    return service.Create(options);
  }
}
