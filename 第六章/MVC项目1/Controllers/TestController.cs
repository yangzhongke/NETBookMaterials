using Microsoft.AspNetCore.Mvc;
using MVC项目1.Models;

namespace MVC项目1.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Demo1(int a)
        {
            var model = new Person("Zack", true, new DateTime(1999, 9, 9));
            return View(model);
        }
        public IActionResult Demo2()
        {
            var model = new Person("Zack", true, new DateTime(1999, 9, 9));
            return View(model);
        }
    }

}
