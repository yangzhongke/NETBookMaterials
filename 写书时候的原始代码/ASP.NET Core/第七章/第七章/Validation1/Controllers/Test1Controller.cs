using Microsoft.AspNetCore.Mvc;

namespace Validation1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult Login1(Login1Request req)
        {
            return Ok(req);
        }

        [HttpPost]
        public IActionResult Login2(Login2Request req)
        {
            return Ok(req);
        }

        [HttpPost]
        public IActionResult Login3(Login3Request req)
        {
            return Ok(req);
        }
    }
}