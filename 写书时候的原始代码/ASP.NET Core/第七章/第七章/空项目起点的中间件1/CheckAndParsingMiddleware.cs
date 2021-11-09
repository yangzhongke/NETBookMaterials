using Dynamic.Json;

namespace 空项目起点的中间件1
{
    public class CheckAndParsingMiddleware
    {
        private readonly RequestDelegate next;

        public CheckAndParsingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string pwd = context.Request.Query["password"];
            if (pwd == "123")
            {
                if (context.Request.HasJsonContentType())
                {
                    //不能直接读Request.Body，否则会直接导致context.Response.StatusCode = 500;出错
                    var reqStream = context.Request.BodyReader.AsStream();
                    dynamic? jsonObj = DJson.Parse(reqStream);
                    context.Items["BodyJson"] = jsonObj;
                }
                await next(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}
