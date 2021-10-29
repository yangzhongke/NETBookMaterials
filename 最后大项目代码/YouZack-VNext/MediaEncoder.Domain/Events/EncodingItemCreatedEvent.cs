using MediaEncoder.Domain.Entities;
using MediatR;

namespace MediaEncoder.Domain.Events;
public record EncodingItemCreatedEvent(EncodingItem Value) : INotification;