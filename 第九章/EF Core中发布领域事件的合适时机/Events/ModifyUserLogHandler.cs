using MediatR;

namespace 领域事件发布的时机1.Events
{
    public class ModifyUserLogHandler : INotificationHandler<UserUpdatedEvent>
    {
        private readonly UserDbContext context;
        private readonly ILogger<ModifyUserLogHandler> logger;

        public ModifyUserLogHandler(UserDbContext context, ILogger<ModifyUserLogHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            //var user = await context.Users.SingleAsync(u=>u.Id== notification.Id);
            var user = await context.Users.FindAsync(notification.Id);
            logger.LogInformation($"通知用户{user.Email}的信息被修改");
        }
    }
}
