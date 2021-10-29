using MediatR;

namespace MediaEncoder.Domain.Events;
public record EncodingItemStartedEvent(Guid Id, string SourceSystem) : INotification;