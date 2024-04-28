using ClassManager.Domain;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ClassManager.Data.Contexts.Accounts.Services;

public class EmailService : IEmailService
{
  public Task Send(string to, string email, string subject, string body, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken)
  {
    return;
    var client = new SendGridClient(Configuration.SendGrid.ApiKey);
    var from = new EmailAddress(Configuration.Email.DefaultFromEmail, Configuration.Email.DefaultFromName);
    const string subject = "Verifique sua conta";
    var to = new EmailAddress(user.Email, user.Name.ToString());
    var content = $"Código {user.Email.Verification.Code}";
    var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
    await client.SendEmailAsync(msg, cancellationToken);
  }
}
