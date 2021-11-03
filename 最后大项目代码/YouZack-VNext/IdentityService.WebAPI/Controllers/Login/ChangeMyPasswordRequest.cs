using FluentValidation;

namespace IdentityService.WebAPI.Controllers.Login;
public record ChangeMyPasswordRequest(string Password, string Password2);
public class ChangePasswordRequestValidator : AbstractValidator<ChangeMyPasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(e => e.Password).NotNull().NotEmpty()
            .Equal(e => e.Password2);
        RuleFor(e => e.Password2).NotNull().NotEmpty();
    }
}
