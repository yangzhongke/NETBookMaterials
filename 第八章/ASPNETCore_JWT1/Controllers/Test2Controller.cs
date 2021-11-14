using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASPNETCore_JWT1.Controllers;
[Route("[controller]/[action]")]
[ApiController]
[Authorize]
public class Test2Controller : ControllerBase
{
	[HttpGet]
	public IActionResult Hello()
	{
		string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		string userName = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
		string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
		return Ok($"id={id},userName={userName},roleNames ={roleNames}");
	}
}