using Microsoft.AspNetCore.Mvc;

namespace webapi响应缓存1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {       

        [HttpGet]
        [ResponseCache(Duration =60)]
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}