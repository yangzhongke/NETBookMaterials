using MediatR;

namespace 领域事件发布的时机1
{
    public record UserSoftDeletedEvent(Guid Id):INotification;
}
