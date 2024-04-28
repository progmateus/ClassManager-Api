using ClassManager.Domain.Shared.ValueObjects;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Shared.ValueObjects
{
  public class Name : ValueObject
  {
    public Name(string firstName, string lastName)
    {
      FirstName = firstName;
      LastName = lastName;

      AddNotifications(new Contract()
        .Requires()
        .HasMinLen(FirstName, 3, "Name.FirstName", "FirstName min 3 characters")
        .HasMaxLen(FirstName, 40, "Name.FirstName", "FirstName max 40 characters")
        .HasMinLen(LastName, 3, "Name.LastName", "LastName min 3 characters")
        .HasMaxLen(LastName, 40, "Name.LastName", "LastName max 40 characters")
      );
    }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }


    public override string ToString()
    {
      return $"{FirstName} {LastName}";
    }
  }
}