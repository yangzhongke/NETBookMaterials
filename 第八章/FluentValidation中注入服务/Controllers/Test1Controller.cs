using Microsoft.AspNetCore.Mvc;

namespace FluentValidation中注入服务.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
        [HttpPost]
        public ActionResult Login3(Login3Request req)
        {
            return Ok();
        }
    }
}