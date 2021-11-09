using Microsoft.AspNetCore.Mvc;
using Zack.EventBus;

namespace EventBus发送1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEventBus eventBus;

        public TestController(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        [HttpPost]
        public IActionResult AddNewUser()
        {
            eventBus.Publish("UserAdded", new {UserName="yzk",Age=18 });
            return Ok();
        }
    }
}
