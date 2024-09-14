using System.Text.Json.Serialization;
using ClassManager.Domain.Shared.ValueObjects;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Shared.ValueObjects
{
  public class Email : ValueObject
  {

    public Email(string address)
    {
      Address = address.ToLower();

      AddNotifications(new Contract()
        .Requires()
        .IsEmail(Address, "Email.Adress", "Invalid email")
        .IsNotNull(Address, "Email.Address", "Email can't be null")
      );
    }

    public string Address { get; }

    [JsonIgnore]
    public Verification Verification { get; private set; } = new();

    public void ResendVerification()
        => Verification = new Verification();

    public static implicit operator string(Email email)
        => email.ToString();

    public static implicit operator Email(string address)
        => new(address);

    public override string ToString()
        => Address;
  }
}