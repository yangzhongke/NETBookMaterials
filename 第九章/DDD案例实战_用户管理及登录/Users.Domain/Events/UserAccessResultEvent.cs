using MediatR;

namespace Users.Domain.Events
{
    public record class UserAccessResultEvent(PhoneNumber PhoneNumber, UserAccessResult Result) : INotification;
}
