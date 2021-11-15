using Users.Domain;

namespace Users.WebAPI.Controllers
{
    public record LoginByPhoneAndPwdRequest(PhoneNumber PhoneNumber,string Password);
}
