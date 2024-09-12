using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Entities
{
  public class User : Entity
  {

    protected User()
    {

    }

    public User(Name name, Document document, Email email, Password password, string username, string? avatar = null)
    {
      Name = name;
      Document = document;
      Email = email;
      Password = password;
      Username = username;
      Avatar = avatar;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;

      AddNotifications(name, document, email);
    }

    public Name Name { get; private set; }
    public Document Document { get; private set; }
    public Password Password { get; private set; }
    public Email Email { get; private set; }
    public string Username { get; private set; }
    public string? Avatar { get; private set; } = string.Empty;
    public EUserStatus Status { get; private set; } = EUserStatus.ACTIVE;
    public EUserType Type { get; private set; } = EUserType.NORMAL;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IList<Role> Roles { get; } = [];
    public IList<UsersRoles> UsersRoles { get; } = [];
    public IList<Class> Classes { get; }
    public IList<TeachersClasses> TeachersClasses { get; } = [];
    public IList<StudentsClasses> StudentsClasses { get; } = [];
    public IList<Subscription> Subscriptions { get; } = [];
    public IList<Booking> Bookings { get; } = [];
    public IList<Tenant> Tenants { get; } = [];

    public void ChangeUser(Name name, Email email, Document document)
    {
      AddNotifications(name, email, document);
      Name = name;
      Email = email;
      Document = document;
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
  }
}