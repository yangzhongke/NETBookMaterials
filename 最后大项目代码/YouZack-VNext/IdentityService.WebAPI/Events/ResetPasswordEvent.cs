namespace IdentityService.WebAPI.Events
{
    public record ResetPasswordEvent(Guid Id, string UserName, string Password, string PhoneNum);
}
