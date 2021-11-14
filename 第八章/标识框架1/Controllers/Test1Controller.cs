using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace 标识框架1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
        private readonly ILogger<Test1Controller> logger;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        public Test1Controller(ILogger<Test1Controller> logger,
            RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.logger = logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
		[HttpPost]
		public async Task<ActionResult> CreateUserRole()
		{
			bool roleExists = await roleManager.RoleExistsAsync("admin");
			if (!roleExists)
			{
				Role role = new Role { Name="Admin"};
				var r = await roleManager.CreateAsync(role);
				if (!r.Succeeded)
				{
					return BadRequest(r.Errors);
				}
			}
			User user = await this.userManager.FindByNameAsync("yzk");
			if (user == null)
			{
				user = new User{UserName="yzk",Email="yangzhongke8@gmail.com",EmailConfirmed=true};
				var r = await userManager.CreateAsync(user, "123456");
				if (!r.Succeeded)
				{
					return BadRequest(r.Errors);
				}
				r = await userManager.AddToRoleAsync(user, "admin");
				if (!r.Succeeded)
				{
					return BadRequest(r.Errors);
				}
			}
			return Ok();
		}

		[HttpPost]
		public async Task<ActionResult> Login(LoginRequest req)
		{
			string userName = req.UserName;
			string password = req.Password;
			var user = await userManager.FindByNameAsync(userName);
			if (user == null)
			{
				return NotFound($"用户名不存在{userName}");
			}
			if (await userManager.IsLockedOutAsync(user))
			{
				return BadRequest("LockedOut");
			}
			var success = await userManager.CheckPasswordAsync(user, password);
			if (success)
			{
				return Ok("Success");
			}
			else
			{
				var r = await userManager.AccessFailedAsync(user);
				if (!r.Succeeded)
				{
					return BadRequest("AccessFailed failed");
				}
				return BadRequest("Failed");
			}
		}

		[HttpPost]
		public async Task<IActionResult> SendResetPasswordToken(
					SendResetPasswordTokenRequest req)
		{
			string email = req.Email;
			var user = await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return NotFound($"邮箱不存在{email}");
			}
			string token = await userManager.GeneratePasswordResetTokenAsync(user);
			logger.LogInformation($"向邮箱{user.Email}发送Token={token}");
			return Ok();
		}
	}
}