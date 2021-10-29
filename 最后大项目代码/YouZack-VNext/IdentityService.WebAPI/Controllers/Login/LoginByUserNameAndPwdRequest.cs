using FluentValidation;

namespace IdentityService.WebAPI.Controllers.Login;
public record LoginByUserNameAndPwdRequest(string UserName, string Password, string Captcha, Guid TicketId);
public class LoginByUserNameAndPwdRequestValidator : AbstractValidator<LoginByUserNameAndPwdRequest>
{
    public LoginByUserNameAndPwdRequestValidator()
    {
        RuleFor(e => e.Captcha).NotNull().NotEmpty();
        RuleFor(e => e.UserName).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();
        RuleFor(e => e.TicketId).NotNull().NotEmpty();
    }
}