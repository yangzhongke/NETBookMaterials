using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptionsSnapshot<SmtpOptions> smtpOptions;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public HomeController(IOptionsSnapshot<SmtpOptions> smtpOptions, IConnectionMultiplexer connectionMultiplexer)
        {
            this.smtpOptions = smtpOptions;
            this.connectionMultiplexer = connectionMultiplexer;
        }

        public IActionResult Index()
        {
            var opt = smtpOptions.Value;
            var timeSpan = connectionMultiplexer.GetDatabase().Ping();
            return Content($"Smtp: {opt} timeSpan:{timeSpan}");
        }
    }
}