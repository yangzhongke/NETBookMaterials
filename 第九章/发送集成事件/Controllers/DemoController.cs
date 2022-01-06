using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zack.EventBus;

namespace 发送集成事件.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private IEventBus eventBus;

        public DemoController(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        [HttpPost]
        public string Publish()
        {
            eventBus.Publish("UserAdded", new { UserName = "yzk", Age = 18 });
            return "ok";
        }
    }
}
