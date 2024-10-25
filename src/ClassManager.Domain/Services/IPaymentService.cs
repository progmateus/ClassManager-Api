using Stripe;

namespace ClassManager.Domain.Services
{
  public interface IPaymentService
  {
    Product CreateProduct(Guid entityId, string ownerType, string name, Guid? tenantId, string? connectedAccountId);
    Subscription CreateSubscription(Guid? tenantId, string stripePriceId, string stripeCustomerId, string? connectedAccountId);
    Customer CreateCustomer(string name, string email, string? connectedAccountId);
    Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents, string? connectedAccountId);
    Invoice CreateInvoice(Guid tenantId, string stripeCustomerId, string stripeSubscriptionId, string? connectedAccountId);
    Account CreateAccount(Guid tenantId, string tenantEmail);
    void CreateWebhook(string? connectedAccountId);
  }
}