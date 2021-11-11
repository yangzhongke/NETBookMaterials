using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using 前后端分离的WebAPI;

[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
	[HttpPost]
	public ActionResult<LoginResult> Login(LoginRequest loginReq)
	{
		if (loginReq.UserName == "admin" && loginReq.Password == "123456")
		{
			var processes = Process.GetProcesses().Select(p => new ProcessInfo(
				p.Id,p.ProcessName,p.WorkingSet64)).ToArray();
			return new LoginResult(true, processes);
		}
		else
		{
			return new LoginResult(false, null);
		}
	}
}