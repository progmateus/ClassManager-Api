using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Services
{
  public interface IEmailService
  {
    Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken);
    Task Send(string to, string email, string subject, string body, CancellationToken cancellationToken);
  }
}