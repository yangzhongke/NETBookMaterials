
using FluentValidation;

namespace IdentityService.WebAPI.Controllers.Login;
public record ChangeMyPasswordRequest(string Password, string Password2, string Captcha, Guid TicketId);
public class ChangePasswordRequestValidator : AbstractValidator<ChangeMyPasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(e => e.Captcha).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();
        RuleFor(e => e.Password2).NotNull().NotEmpty();
        RuleFor(e => e.TicketId).NotNull().NotEmpty();
        RuleFor(e => e.Password).Equal(e => e.Password2);
    }
}
