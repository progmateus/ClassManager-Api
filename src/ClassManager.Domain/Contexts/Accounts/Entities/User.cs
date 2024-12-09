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
  public class User : Entity
  {

    protected User()
    {

    }

    public User(Name name, Document document, Email email, Password password, string username, string? phone, string? avatar = null)
    {
      Name = name;
      Document = document;
      Email = email;
      Password = password;
      Username = username;
      Phone = phone;
      Avatar = avatar;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;

      AddNotifications(name, document, email);
    }

    public Name Name { get; private set; }
    public Document Document { get; private set; }
    [JsonIgnore]
    public Password Password { get; private set; }
    public Email Email { get; private set; }
    public string Username { get; private set; }
    public string? Avatar { get; private set; } = string.Empty;
    public string? Phone { get; private set; } = string.Empty;
    public EUserStatus Status { get; private set; } = EUserStatus.ACTIVE;
    public string? StripeCustomerId { get; private set; }

    public EUserType Type { get; private set; } = EUserType.NORMAL;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Address? Address { get; private set; }
    public IList<Role> Roles { get; private set; } = [];
    public IList<UsersRoles> UsersRoles { get; private set; } = [];
    public IList<Class> Classes { get; private set; } = [];
    public IList<TeachersClasses> TeachersClasses { get; private set; } = [];
    public IList<StudentsClasses> StudentsClasses { get; private set; } = [];
    public IList<Subscription> Subscriptions { get; private set; } = [];
    public IList<Booking> Bookings { get; private set; } = [];
    public IList<Tenant> Tenants { get; private set; } = [];
    public IList<Invoice> Invoices { get; private set; } = [];
    public IList<StripeCustomer> StripeCustomers { get; private set; } = [];

    public void ChangeUser(Name name, Email email, Document document, string? phone)
    {
      AddNotifications(name, email, document);
      Name = name;
      Email = email;
      Document = document;
      Phone = phone;
    }

    public void UpdatePassword(string plainTextPassword, string code)
    {
      if (!string.Equals(code.Trim(), Password.ResetCode.Trim(), StringComparison.CurrentCultureIgnoreCase))
        throw new Exception("Código de restauração inválido");

      var password = new Password(plainTextPassword);
      Password = password;
    }

    public void ChangeEmail(Email email)
    {
      Email = email;
    }

    public void ChangePassword(string plainTextPassword)
    {
      var password = new Password(plainTextPassword);
      Password = password;
    }

    public void Delete()
    {
      Status = EUserStatus.DELETED;
    }

    public void SetStripeCustomerId(string stripeCustomerId)
    {
      StripeCustomerId = stripeCustomerId;
    }
  }
}