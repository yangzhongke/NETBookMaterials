using MediatR;

namespace 领域事件1
{
    public class TestEventHandler1 : INotificationHandler<TestEvent>
    {
        public Task Handle(TestEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"我收到了{notification.UserName}");
            return Task.CompletedTask;
        }
    }
}
