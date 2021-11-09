using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Validation1.Controllers
{
    public class Login3Request
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Login3RequestValidator:AbstractValidator<Login3Request>
    {
        public Login3RequestValidator(TestDbContext dbCtx)
        {
            /*
            RuleFor(x => x.UserName).NotNull()
                .Must(name=>dbCtx.Users.Any(u=>u.UserName== name))
                .WithMessage(c => $"用户名{c.UserName}不存在");*/
            RuleFor(x => x.UserName).NotNull()
                .MustAsync((name,_) => dbCtx.Users.AnyAsync(u => u.UserName == name))
                .WithMessage(c => $"用户名{c.UserName}不存在");
        }
    }
}
