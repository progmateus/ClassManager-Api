using Stripe;

namespace ClassManager.Domain.Services
{
  public interface IPaymentService
  {
    Product CreateProduct(Guid entityId, string ownerType, string name, Guid? tenantId);
    Subscription CreateSubscription(Guid tenantId, string stripePriceId, string stripeCustomerId);
    Customer CreateCustomer(string name, string email);
    Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents);
    Invoice CreateInvoice(Guid tenantId, string stripeCustomerId, string stripeSubscriptionId);
    Account CreateAccount(Guid tenantId, string tenantEmail);
    void RequestUsingConnectedAccount();
  }
}