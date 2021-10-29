using MediatR;

namespace Listening.Domain.Events
{
    public record EpisodeDeletedEvent(Guid Id) : INotification;
}
