using Microsoft.AspNetCore.Mvc;

namespace 客户端响应缓存.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class Test1Controller : ControllerBase
    {
        [HttpGet]
        public DateTime Now()
        {
            return DateTime.Now;
        }

    }
}
