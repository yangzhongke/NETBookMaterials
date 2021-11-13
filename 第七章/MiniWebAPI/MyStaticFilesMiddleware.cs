using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MiniWebAPI
{
    /// <summary>
    /// 对网站中的静态文件进行处理的中间件
    /// </summary>
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
            string path = context.Request.Path.Value ?? "";
            //根据路径获取wwwroot文件夹下的静态文件
            var file = hostEnv.WebRootFileProvider.GetFileInfo(path);
            if (!file.Exists||!ContentTypeHelper.IsValid(file))
            {
                await next(context);
                return;
            }
            context.Response.ContentType = ContentTypeHelper.GetContentType(file);
            context.Response.StatusCode = 200;
            using var stream = file.CreateReadStream();
            byte[] bytes = await ToArrayAsync(stream);
            await context.Response.Body.WriteAsync(bytes);
        }

        private static async Task<byte[]> ToArrayAsync(Stream stream)
        {
            using MemoryStream memStream = new MemoryStream();
            await stream.CopyToAsync(memStream);
            memStream.Position = 0;
            byte[] bytes = memStream.ToArray();
            return bytes;
        }
    }
}
