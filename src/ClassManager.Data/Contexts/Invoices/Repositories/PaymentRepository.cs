using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class PaymentRepository : TRepository<Payment>, IPaymentRepository
{
  public PaymentRepository(AppDbContext context) : base(context) { }
}
