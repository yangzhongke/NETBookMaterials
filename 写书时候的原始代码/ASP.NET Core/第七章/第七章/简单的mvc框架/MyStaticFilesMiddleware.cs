namespace 简单的mvc框架
{
    public class MyStaticFilesMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment hostEnv;

        public MyStaticFilesMiddleware(RequestDelegate next, IWebHostEnvironment hostEnv)
        {
            this.next = next;
            this.hostEnv = hostEnv;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? path = context.Request.Path.Value;
            if(path!=null&&path.StartsWith("/"))
            {
                path = path.Substring(1);//删除开头的/
            }
            if(string.IsNullOrEmpty(path))
            {
                await next(context);
                return;
            }            
            string contentRootPath = hostEnv.ContentRootPath;
            string localFilePath = Path.Combine(contentRootPath,"www", path);
            FileInfo file = new FileInfo(localFilePath);
            if (!file.Exists||!ContentTypeHelper.IsValid(file))
            {
                await next(context);
                return;
            }
            context.Response.ContentType = ContentTypeHelper.GetContentType(file);
            context.Response.StatusCode = 200;
            byte[] bytes = await File.ReadAllBytesAsync(localFilePath);
            await context.Response.Body.WriteAsync(bytes);
        }
    }
}
