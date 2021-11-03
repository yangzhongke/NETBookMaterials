using FluentValidation;

namespace IdentityService.WebAPI.Controllers.Login;
public record LoginByUserNameAndPwdRequest(string UserName, string Password);
public class LoginByUserNameAndPwdRequestValidator : AbstractValidator<LoginByUserNameAndPwdRequest>
{
    public LoginByUserNameAndPwdRequestValidator()
    {
        RuleFor(e => e.UserName).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();
    }
}