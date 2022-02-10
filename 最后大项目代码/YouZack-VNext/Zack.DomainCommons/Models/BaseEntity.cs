using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zack.DomainCommons.Models
{
    public record BaseEntity : IEntity, IDomainEvents
    {

        [NotMapped]
        private List<INotification> domainEvents = new();

        public Guid Id { get; protected set; } = Guid.NewGuid();

        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents.Add(eventItem);
        }

        public void AddDomainEventIfAbsent(INotification eventItem)
        {
            if (!domainEvents.Contains(eventItem))
            {
                domainEvents.Add(eventItem);
            }
        }
        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return domainEvents;
        }
    }
}
