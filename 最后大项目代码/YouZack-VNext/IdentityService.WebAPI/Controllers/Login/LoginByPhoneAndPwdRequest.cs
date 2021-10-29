
using FluentValidation;

namespace IdentityService.WebAPI.Controllers.Login;
public record LoginByPhoneAndPwdRequest(string PhoneNum, string Password, string Captcha, Guid TicketId);
public class LoginByPhoneAndPwdRequestValidator : AbstractValidator<LoginByPhoneAndPwdRequest>
{
    public LoginByPhoneAndPwdRequestValidator()
    {
        RuleFor(e => e.Captcha).NotNull().NotEmpty();
        RuleFor(e => e.PhoneNum).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();
        RuleFor(e => e.TicketId).NotNull().NotEmpty();
    }
}