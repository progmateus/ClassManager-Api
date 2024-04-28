using System.Linq.Expressions;
using ClassManager.Domain.Shared.Entities;

public abstract class BaseService
{
  protected bool Validate<TV, TE>(TV validator, TE entity)
  where TE : Entity
  {
    /* var validaor = validator.Validate(entity);

    if (validaor.IsValid)
    {
      return true;
    } */
    // lançamento das notificações'
    return true;
  }

}