using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Services;

namespace ClassManager.Tests.Mocks
{
    public class FakeEmailService : IEmailService
    {
        public Task Send(string to, string email, string subject, string body, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}