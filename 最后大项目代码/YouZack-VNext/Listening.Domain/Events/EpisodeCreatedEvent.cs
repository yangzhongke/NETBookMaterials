using Listening.Domain.Entities;
using MediatR;

namespace Listening.Domain.Events
{
    public record EpisodeCreatedEvent(Episode Value) : INotification;
}
