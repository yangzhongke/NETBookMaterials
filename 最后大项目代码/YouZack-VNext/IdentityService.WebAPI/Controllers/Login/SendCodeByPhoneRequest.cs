
using FluentValidation;

namespace IdentityService.WebAPI.Controllers.Login;
public record SendCodeByPhoneRequest(string PhoneNumber, string Captcha, Guid TicketId);
public class SendCodeByPhoneRequestValidator : AbstractValidator<SendCodeByPhoneRequest>
{
    public SendCodeByPhoneRequestValidator()
    {
        RuleFor(e => e.Captcha).NotNull().NotEmpty();
        RuleFor(e => e.PhoneNumber).NotNull().NotEmpty();
        RuleFor(e => e.TicketId).NotNull().NotEmpty();
    }
}