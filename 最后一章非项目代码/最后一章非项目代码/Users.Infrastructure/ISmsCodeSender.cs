using Users.Domain;

namespace Users.Infrastructure
{
    public interface ISmsCodeSender
    {
        Task SendCodeAsync(PhoneNumber phoneNumber,string code);
    }
}
