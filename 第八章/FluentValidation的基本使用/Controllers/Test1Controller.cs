using Microsoft.AspNetCore.Mvc;

namespace FluentValidation的基本使用.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
        [HttpPost]
        public ActionResult Login(Login2Request req)
        {
            return Ok();
        }
    }
}