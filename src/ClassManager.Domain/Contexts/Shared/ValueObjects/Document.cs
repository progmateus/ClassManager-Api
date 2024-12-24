using System.Text.RegularExpressions;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.ValueObjects;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Shared.ValueObjects
{
  public class Document : ValueObject
  {
    public Document(string number)
    {
      Number = Regex.Replace(number, "/W/g", "");
      AddNotifications(new Contract()
      .Requires()
      .IsTrue(Validate(), "Document.Number", "Invalid document")
      );
    }

    public string Number { get; private set; }
    public EDocumentType Type { get; private set; } = EDocumentType.CPF;

    private bool Validate()
    {
      var cpfRegex = new Regex("(^d{3}.?d{3}.?d{3}-?d{2}$)", RegexOptions.IgnoreCase);
      var cnpjRegex = new Regex("^[0-9]{2}.?[0-9]{3}.?[0-9]{3}/?[0-9]{4}-?[0-9]{2}$", RegexOptions.IgnoreCase);

      if (cpfRegex.IsMatch(Number))
      {
        Type = EDocumentType.CPF;
        return true;
      }

      if (cnpjRegex.IsMatch(Number))
      {
        Type = EDocumentType.CNPJ;
        return true;
      }
      return false;
    }

    public static implicit operator string(Document document)
        => document.ToString();

    public override string ToString()
      => Number;
  }
}