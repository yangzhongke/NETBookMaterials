using MediatR;

namespace MediaEncoder.Domain.Events;
public record EncodingItemCompletedEvent(Guid Id, string SourceSystem, Uri OutputUrl) : INotification;