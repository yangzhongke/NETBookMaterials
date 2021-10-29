using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace 筛选器1
{
    public class RateLimitFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache memCache;

        public RateLimitFilter(IMemoryCache memCache)
        {
            this.memCache = memCache;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string removeIP = context.HttpContext.Connection.RemoteIpAddress.ToString();
            string cacheKey = $"LastVisitTick_{removeIP}";
            long? lastTick = memCache.Get<long?>(cacheKey);
            if(lastTick==null||Environment.TickCount64 - lastTick>1000)
            {
                memCache.Set(cacheKey, Environment.TickCount64,
                    TimeSpan.FromSeconds(10));
                return next();
            }
            else
            {
                var result = new ContentResult();
                result.StatusCode = (int)HttpStatusCode.TooManyRequests;
                result.ContentType = "text/html";
                result.Content = "Only once per second!";
                context.Result = result;
                return Task.CompletedTask;
            }
        }
    }
}
