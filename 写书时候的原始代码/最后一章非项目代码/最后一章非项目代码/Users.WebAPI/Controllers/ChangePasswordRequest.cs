using Users.Domain;

namespace Users.WebAPI.Controllers
{
    public record ChangePasswordRequest(Guid Id,string Password);
}
