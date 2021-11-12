using MediatR;
using System;
using System.Collections.Generic;

namespace Zack.DomainCommons.Models
{
    public class BaseEntity : IEntity, IDomainEvents
    {

        /*
        [NotMapped]//这样有点不好，通过代码配置方式来降低依赖，单一职责。不过影响不大，所以就用Attribute吧
        public List<INotification> DomainEvents { get; }
            = new List<INotification>();*/
        //好像在.NET 5中，这个DomainEvents必须为public 属性+[NotMapped]，因为ChangeTracker跟踪的是一个拷贝，而不是本来这个对象
        //但是好像到了.NET6中，DomainEvents为字段也可以了
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
