using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Contexts.Accounts.Entities
{
  public class User : Entity
  {

    protected User()
    {

    }

    public User(Name name, Document document, Email email, Password password, string? avatar = null)
    {
      Name = name;
      Document = document;
      Email = email;
      Password = password;
      Avatar = avatar;
      CreatedAt = DateTime.Now;
      UpdatedAt = DateTime.Now;

      AddNotifications(name, document, email);
    }

    public Name Name { get; private set; }
    public Document Document { get; private set; }
    public Password Password { get; private set; }
    public Email Email { get; private set; }
    public string? Avatar { get; private set; } = string.Empty;
    public EUserStatus Status { get; private set; } = EUserStatus.ACTIVE;
    public EUserType Type { get; private set; } = EUserType.NORMAL;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public List<Role> Roles { get; } = new();

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
  }
}