using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Stripe;

namespace ClassManager.Domain.Services
{
  public interface IStripeService
  {
    Product CreateProduct(Guid entityId, string ownerType, string name, Guid? tenantId);
    Subscription CreateSubscription(Guid tenantId, string stripePriceId, string stripeCustomerId);
    Customer CreateCustomer(Tenant tenant);
    Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents);
  }
}