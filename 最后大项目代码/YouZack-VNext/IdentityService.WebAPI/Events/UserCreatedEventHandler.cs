using Identity.Repository;
using Microsoft.Extensions.Logging;
using Zack.EventBus;

namespace IdentityService.WebAPI.Events
{
    [EventName("IdentityService.User.Created")]
    public class UserCreatedEventHandler : JsonIntegrationEventHandler<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedEventHandler> logger;
        private readonly ISmsSender smsSender;

        public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, ISmsSender smsSender)
        {
            this.logger = logger;
            this.smsSender = smsSender;
        }

        public override Task HandleJson(string eventName, UserCreatedEvent? eventData)
        {
            //发送初始密码给被创建用户的手机
            return smsSender.SendAsync(eventData.PhoneNum, eventData.Password);
        }
    }
}