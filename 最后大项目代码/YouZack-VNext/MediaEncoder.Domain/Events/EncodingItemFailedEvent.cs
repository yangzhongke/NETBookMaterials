using MediatR;

namespace MediaEncoder.Domain.Events;
public record EncodingItemFailedEvent(Guid Id, string SourceSystem, string ErrorMessage) : INotification;