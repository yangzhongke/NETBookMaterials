using Microsoft.AspNetCore.Http;

namespace MiniWebAPI
{
    /// <summary>
    /// 如果一个请求不能被管道中任何一个中间件处理，也就是请求的地址不存在，
    /// 则ASP.NET Core会向客户端写入状态码为404的响应。
    /// 为了能够显示自定义的报错信息，我开发了这个中间件类。
    /// 我们一般把其他中间件放到前面，而把NotFoundMiddleware放到管道中的最后。
    /// 这样，如果任何一个中间件能够处理这个请求，这个请求都会被处理，
    /// 然后短路请求，而如果没有任何一个中间件能够处理这个请求，
    /// 这个请求就会最终由NotFoundMiddleware处理。
    /// </summary>
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/html;charset=utf-8";
            await context.Response.WriteAsync("请求来到了一片未知的荒原");
        }
    }
}
