using Listening.Domain.Entities;
using MediatR;

namespace Listening.Domain.Events
{
    public record EpisodeUpdatedEvent(Episode Value) : INotification;
}
