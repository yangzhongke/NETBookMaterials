
using FluentValidation;

namespace IdentityService.WebAPI.Controllers.Login;
public record LoginByPhoneAndCodeRequest(string PhoneNum, string Code, string Captcha, Guid TicketId);
public class LoginByPhoneAndCodeRequestValidator : AbstractValidator<LoginByPhoneAndCodeRequest>
{
    public LoginByPhoneAndCodeRequestValidator()
    {
        RuleFor(e => e.Captcha).NotNull().NotEmpty();
        RuleFor(e => e.PhoneNum).NotNull().NotEmpty();
        RuleFor(e => e.Code).NotNull().NotEmpty();
        RuleFor(e => e.TicketId).NotNull().NotEmpty();
    }
}