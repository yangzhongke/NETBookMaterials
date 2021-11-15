using Users.Domain;

namespace Users.WebAPI.Controllers
{
    //暂时不考虑暴力请求的问题
    public record SendLoginByPhoneAndCodeRequest(PhoneNumber PhoneNumber);
}
