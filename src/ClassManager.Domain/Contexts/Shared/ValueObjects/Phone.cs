using System.Text.RegularExpressions;
using ClassManager.Domain.Shared.ValueObjects;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Shared.ValueObjects
{
  public class Phone : ValueObject
  {
    public Phone(string number)
    {
      Number = Regex.Replace(number, "/W/g", "");
      AddNotifications(new Contract()
      .Requires()
      .IsTrue(Validate(), "Phone.Number", "Invalid phone")
      );
    }

    public string Number { get; private set; }

    private bool Validate()
    {
      var phoneRegex = new Regex("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$", RegexOptions.IgnoreCase);

      return phoneRegex.IsMatch(Number);
    }

    public static implicit operator string(Phone phone)
        => phone.ToString();

    public static implicit operator Phone(string phone)
    => new(phone);

    public override string ToString()
      => Number;
  }
}