using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using WebAPI1.Models;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController: ControllerBase
    {
        [HttpPost]
        public ActionResult<LoginResult> Login(LoginRequest loginReq)
        {
            LoginResult loginResult = new LoginResult();
            loginResult.UserName = loginReq.UserName;
            if (loginReq.UserName == "admin" && loginReq.Password == "123456")
            {
                loginResult.IsOK = true;
                var processes = Process.GetProcesses().Select(p=>new ProcessInfo {Id= p.Id,ProcessName=p.ProcessName,WorkingSet64= p.WorkingSet64});//获取当前所有进程
                loginResult.Processes = processes.ToArray();
            }
            else
            {
                loginResult.IsOK = false;
            }
            return loginResult;
        }
    }
}
