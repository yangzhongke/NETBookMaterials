using MediaEncoder.Domain.Events;
using Zack.EventBus;

namespace MediaEncoder.WebAPI.EventHandlers;
class EncodingItemCompletedEventHandler : INotificationHandler<EncodingItemCompletedEvent>
{
    private readonly IEventBus eventBus;

    public EncodingItemCompletedEventHandler(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public Task Handle(EncodingItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        eventBus.Publish("MediaEncoding.Completed", notification);
        return Task.CompletedTask;
    }
}