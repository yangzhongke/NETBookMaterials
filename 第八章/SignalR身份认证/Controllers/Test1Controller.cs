using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalRCoreTest2;
using SignalRCoreTest2.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalRCoreTest1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
		private readonly IHubContext<ChatRoomHub> hubContext;
		public Test1Controller(IHubContext<ChatRoomHub> hubContext)
		{
			this.hubContext = hubContext;
		}

		private static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
		{
			DateTime expires = DateTime.Now.AddSeconds(options.ExpireSeconds);
			byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey);
			var secKey = new SymmetricSecurityKey(keyBytes);
			var credentials = new SigningCredentials(secKey,
				SecurityAlgorithms.HmacSha256Signature);
			var tokenDescriptor = new JwtSecurityToken(expires: expires,
				signingCredentials: credentials, claims: claims);
			return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest req,
					[FromServices] IOptions<JWTOptions> jwtOptions)
		{
			string userName = req.UserName;
			string password = req.Password;			
			User? user = UserManager.FindByName(userName);
			if (user==null|| user.Password!=password)
            {
				return BadRequest("用户名或者密码错误");
			}
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Name, userName));
			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			string jwtToken = BuildToken(claims, jwtOptions.Value);
			return Ok(jwtToken);
		}

		[HttpPost]
		public async Task<IActionResult> AddUser(AddNewUserRequest req)
		{
			//这里省略执行用户注册的代码
			await hubContext.Clients.All.SendAsync("UserAdded", req.UserName);
			return Ok();
		}

	}
}