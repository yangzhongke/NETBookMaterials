using Users.Domain;

namespace Users.Domain
{
    public interface ISmsCodeSender
    {
        Task SendCodeAsync(PhoneNumber phoneNumber,string code);
    }
}
