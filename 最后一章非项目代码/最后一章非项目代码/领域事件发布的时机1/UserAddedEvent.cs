using MediatR;

namespace 领域事件发布的时机1
{
    public record UserAddedEvent(User Item):INotification;
}
