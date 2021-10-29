using Identity.Repository;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Services
{
    public class MockEmailSender : IEmailSender
    {
        private readonly ILogger<MockEmailSender> logger;
        public MockEmailSender(ILogger<MockEmailSender> logger)
        {
            this.logger = logger;
        }

        public Task SendAsync(string toEmail, string subject, string body)
        {
            logger.LogInformation("Send Email to {0},title:{1}, body:{2}", toEmail, subject, body);
            return Task.CompletedTask;
        }
    }
}
