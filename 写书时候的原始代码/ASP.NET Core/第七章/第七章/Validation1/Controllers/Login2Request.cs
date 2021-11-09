using FluentValidation;

namespace Validation1.Controllers
{
    public class Login2Request
    {
        public string Email { get; set; }
        public string Password { get; set; }    
        public string Password2 { get; set; }
    }

    public class Login2RequestValidator: AbstractValidator<Login2Request>
    {
        public Login2RequestValidator()
        {
            RuleFor(x=>x.Email).NotNull().NotEmpty()
                .EmailAddress()
                .Must(v=>v.EndsWith("@qq.com")||v.EndsWith("@163.com"))
                .WithMessage("只支持QQ和163邮箱");
            RuleFor(x => x.Password).NotNull().NotEmpty()
                .Length(3, 10)
                .WithMessage("密码长度必须介于3到10之间")
                .Equal(x => x.Password2)
                .WithMessage("两次密码必须一致");
        }
    }
}
