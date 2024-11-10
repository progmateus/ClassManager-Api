using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.ValueObjects;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Shared.ValueObjects
{
  public class Document : ValueObject
  {
    public Document(string number, EDocumentType type)
    {
      Number = number.Replace(".", "").Replace("-", "").Replace(" ", "");
      Type = type;

      AddNotifications(new Contract()
      .Requires()
      .IsTrue(Validate(), "Document.Number", "Invalid document")
      );
    }

    public string Number { get; private set; }
    public EDocumentType Type { get; private set; }

    private bool Validate()
    {
      if (Type == EDocumentType.CNPJ && Number.Length == 14)
      {
        return true;
      }

      if (Type == EDocumentType.CPF && Number.Length == 11)
      {
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