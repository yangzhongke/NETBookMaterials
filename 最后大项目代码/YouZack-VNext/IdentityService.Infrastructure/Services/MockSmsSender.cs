using Identity.Repository;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Services
{
    public class MockSmsSender : ISmsSender
    {
        private readonly ILogger<MockSmsSender> logger;
        public MockSmsSender(ILogger<MockSmsSender> logger)
        {
            this.logger = logger;
        }
        public Task SendAsync(string phoneNum, params string[] args)
        {
            logger.LogInformation("Send Sms to {0},args:{1}", phoneNum,
                 string.Join(",", args));
            return Task.CompletedTask;
        }
    }
}
