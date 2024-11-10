using Stripe;
using Stripe.Identity;
using Address = ClassManager.Domain.Contexts.Shared.ValueObjects.Address;

namespace ClassManager.Domain.Services.Stripe.Repositories.Contracts
{
  public interface IPaymentService
  {
    Product CreateProduct(Guid entityId, string ownerType, string name, Guid? tenantId, string? connectedAccountId);
    Subscription CreateSubscription(Guid? tenantId, string stripePriceId, string stripeCustomerId, string? connectedAccountId);
    Customer CreateCustomer(string name, string email, string? connectedAccountId);
    Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents, string? connectedAccountId);
    Invoice CreateInvoice(Guid tenantId, string stripeCustomerId, string stripeSubscriptionId, string? connectedAccountId);
    Account CreateAccount(string email);
    Subscription ResumeSubscription(string stripeSubscriptionId, string? connectedAccountId);
    Subscription CancelSubscription(string stripeSubscriptionId, string? connectedAccountId);
    void CreateBankAccount(string number, string country, string currency, string holderName, string connectedAccountId);
    void AcceptStripeTerms(string ip, string connectedAccountId);
    void CreateWebhook();
    VerificationSession CreateVerificationSession(string email, string connectedAccountId);
    AccountLink CreateAccountLink(string connectedAccountId, string? linkType = "account_onboarding");
  }
}