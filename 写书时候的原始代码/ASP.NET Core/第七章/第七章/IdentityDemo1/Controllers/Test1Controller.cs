using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityDemo1.Controllers
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

        [HttpGet]
        public async Task<IActionResult> CreateUserRole()
        {
            bool roleExists = await roleManager.RoleExistsAsync("admin");
            if(!roleExists)
            {
                Role role = new Role();
                role.Name = "admin";
                var r = await roleManager.CreateAsync(role);
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
            }
            User user = await this.userManager.FindByNameAsync("yzk");
            if (user == null)
            {
                user = new User();
                user.UserName = "yzk";
                user.NickName = "快乐的程序员";
                user.CreationTime = DateTime.Now;
                user.Email = "yangzhongke8@gmail.com";
                user.EmailConfirmed = true;
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
        public async Task<IActionResult> Login(LoginRequest req)
        {
            string userName = req.UserName;
            string password = req.Password;
            var user=await userManager.FindByNameAsync(userName);
            if(user==null)
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
                //并没有完成真正的“登录”，只是检查密码正确，后续还需要结合JWT、Cookie等
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

        private static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
        {
            DateTime expires = DateTime.Now.AddSeconds(options.ExpireSeconds);
            byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(expires: expires, signingCredentials: credentials,claims:claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        [HttpPost]
        public async Task<IActionResult> Login2(LoginRequest req, 
            [FromServices]IOptions<JWTOptions> jwtOptions)
        {
            //暂时省略对于Lockout的处理
            string userName = req.UserName;
            string password = req.Password;
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"用户名不存在{userName}");
            }
            var success = await userManager.CheckPasswordAsync(user, password);
            if (!success)
            {
                return BadRequest("Failed");
            }
            user.JWTVersion++;
            await userManager.UpdateAsync(user);

            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Version, 
                user.JWTVersion.ToString()));
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            string jwtToken = BuildToken(claims, jwtOptions.Value);
            return Ok(jwtToken);
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordToken(SendResetPasswordTokenRequest req)
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

        [HttpPost]
        public async Task<IActionResult> VerifyResetPasswordToken(VerifyResetPasswordTokenRequest req)
        {
            if(req.NewPassword!=req.NewPassword2)
            {
                return BadRequest("两次密码不一致");
            }
            string email = req.Email;
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"邮箱不存在{email}");
            }
            string token = req.Token;
            string password = req.NewPassword;
            var r = await userManager.ResetPasswordAsync(user, token, password);
            if(r.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(r.Errors);
            }          
        }
    }
}