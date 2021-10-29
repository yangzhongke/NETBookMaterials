namespace IdentityService.WebAPI.Events
{
    public record UserCreatedEvent(Guid Id, string UserName, string Password, string PhoneNum);
}
