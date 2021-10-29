using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace 领域事件1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            //不要写成Send
            await mediator.Publish(new TestEvent(req.UserName));
            return Ok("ok");
        }
    }
}
