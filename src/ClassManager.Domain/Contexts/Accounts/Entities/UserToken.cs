using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using System.Text.Json.Serialization;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Addresses.Entites;

namespace ClassManager.Domain.Contexts.Accounts.Entities
{
  public class UserToken : Entity
  {

    protected UserToken()
    {

    }

    public UserToken(Guid userId, string refreshToken, DateTime expiresAt)
    {
      UserId = userId;
      RefreshToken = refreshToken;
      ExpiresAt = expiresAt;
    }

    public string RefreshToken { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public DateTime ExpiresAt { get; private set; }
  }
}