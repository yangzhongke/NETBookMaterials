using MediatR;

namespace 领域事件发布的时机1
{
    public interface IDomainEvents
    {
        IEnumerable<INotification> GetDomainEvents();
        void AddDomainEvent(INotification eventItem);
        void AddDomainEventIfAbsent(INotification eventItem);
        void ClearDomainEvents();
    }
}
