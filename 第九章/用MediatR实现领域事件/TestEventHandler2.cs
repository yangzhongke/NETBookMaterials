using MediatR;

namespace 领域事件1
{
    public class TestEventHandler2 : INotificationHandler<TestEvent>
    {
        public async Task Handle(TestEvent notification, CancellationToken cancellationToken)
        {
            await File.WriteAllTextAsync("d:/1.txt", $"来了{notification.UserName}");
        }
    }
}
