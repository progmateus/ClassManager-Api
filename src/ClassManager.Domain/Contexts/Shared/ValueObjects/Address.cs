using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.ValueObjects;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Shared.ValueObjects
{
  public class Address : ValueObject
  {
    public Address(string street, string number, string neighborhood, string city, string state, string country, string zipCode)
    {
      Street = street;
      Number = number;
      Neighborhood = neighborhood;
      City = city;
      State = state;
      Country = country;
      ZipCode = zipCode;

      AddNotifications(new Contract()
        .Requires()
        .HasMinLen(Street, 3, "Address.Street", "Street min 3 characters")
        .HasMaxLen(Street, 40, "Address.Street", "Street max 40 characters")
      );
    }

    public Address(string street, string city, string state, string zipCode)
    {
      Street = street;
      City = city;
      State = state;
      ZipCode = zipCode;

      AddNotifications(new Contract()
        .Requires()
        .IsNotNullOrEmpty(City, "City", "City cannot be null")
        .IsNotNullOrEmpty(State, "State", "State cannot be null")
        .IsNotNullOrEmpty(ZipCode, "ZipCode", "ZipCode cannot be null")
        .IsNotNullOrEmpty(Street, "Street", "Street cannot be null")
      );
    }
    public string Street { get; private set; }
    public string? Number { get; private set; }
    public string? Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; } = "BR";
    public string ZipCode { get; private set; }
  }
}