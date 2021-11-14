using Microsoft.AspNetCore.Mvc;

namespace 内置数据校验.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public ActionResult Login1(Login1Request req)
        {
            return Ok(req);
        }

    }
}