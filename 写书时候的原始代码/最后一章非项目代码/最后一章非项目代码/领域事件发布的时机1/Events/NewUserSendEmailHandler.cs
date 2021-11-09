using MediatR;

namespace 领域事件发布的时机1.Events
{
    public class NewUserSendEmailHandler : INotificationHandler<UserAddedEvent>
    {
        private readonly ILogger<NewUserSendEmailHandler> logger;

        public NewUserSendEmailHandler(ILogger<NewUserSendEmailHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(UserAddedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.Item;
            logger.LogInformation($"向{user.Email}发送欢迎邮件");
            return Task.CompletedTask;
        }
    }
}
