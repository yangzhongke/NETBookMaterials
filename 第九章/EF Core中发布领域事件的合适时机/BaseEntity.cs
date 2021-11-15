using MediatR;

namespace 领域事件发布的时机1
{
    public abstract class BaseEntity : IDomainEvents
    {
        private List<INotification> DomainEvents = new();

        public void AddDomainEvent(INotification eventItem)
        {
            DomainEvents.Add(eventItem);
        }

        public void AddDomainEventIfAbsent(INotification eventItem)
        {
            if (!DomainEvents.Contains(eventItem))
            {
                DomainEvents.Add(eventItem);
            }
        }

        public void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return DomainEvents;
        }
    }
}
