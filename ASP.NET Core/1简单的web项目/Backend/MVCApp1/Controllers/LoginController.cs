using Microsoft.AspNetCore.Mvc;
using MVCApp1.Models;
using System.Diagnostics;
using System.Linq;

namespace MVCApp1.Controllers
{
    public class LoginController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(LoginRequest loginReq)
        {
            LoginResult loginResult = new LoginResult();
            loginResult.UserName = loginReq.UserName;
            if(loginReq.UserName=="admin"&&loginReq.Password=="123456")
            {
                loginResult.IsOK = true;
                var processes = Process.GetProcesses().Select(p => new ProcessInfo { Id = p.Id, ProcessName = p.ProcessName, WorkingSet64 = p.WorkingSet64 });//获取当前所有进程//获取当前所有进程
                loginResult.Processes = processes.ToArray();
            }
            else
            {
                loginResult.IsOK = false;
            }
            return View(loginResult);
        }
    }
}
