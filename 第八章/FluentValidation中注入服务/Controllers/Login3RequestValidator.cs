using FluentValidation;
using FluentValidation中注入服务.Controllers;

public class Login3RequestValidator : AbstractValidator<Login3Request>
{
	public Login3RequestValidator(TestDbContext dbCtx)
	{
		RuleFor(x => x.UserName).NotNull()
			.Must(name => dbCtx.Users.Any(u => u.UserName == name))
			.WithMessage(c => $"用户名{c.UserName}不存在");
	}
}