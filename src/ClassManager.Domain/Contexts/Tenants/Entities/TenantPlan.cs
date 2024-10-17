using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Invoices.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class TenantPlan : Entity
  {

    protected TenantPlan()
    {

    }
    public TenantPlan(string name, string description, int timesOfweek, Guid tenantId, decimal price)
    {
      Name = name;
      Description = description;
      TimesOfweek = timesOfweek;
      TenantId = tenantId;
      Price = price;
    }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; }
    public int TimesOfweek { get; private set; }
    public decimal Price { get; private set; }
    public string? StripeProductId { get; private set; }
    public string? StripePriceId { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant? Tenant { get; private set; }
    public ICollection<Subscription> Subscriptions { get; } = [];
    public IList<Invoice> Invoices { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void ChangeTenantPlan(string name, string description, int timesOfweek, decimal price)
    {
      Name = name;
      Description = description;
      TimesOfweek = timesOfweek;
      Price = price;
    }

    public void SetStripeProductId(string stripeCustomerId)
    {
      StripeProductId = stripeCustomerId;
    }

    public void SetStripePriceId(string stripePriceId)
    {
      StripePriceId = stripePriceId;
    }
  }
}