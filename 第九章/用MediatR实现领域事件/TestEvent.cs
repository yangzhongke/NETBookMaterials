using MediatR;

namespace 领域事件1
{
    public record TestEvent(string UserName) : INotification;
}
