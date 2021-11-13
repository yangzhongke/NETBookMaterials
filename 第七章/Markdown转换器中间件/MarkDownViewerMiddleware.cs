using MarkdownSharp;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using Ude;

public class MarkDownViewerMiddleware
{
    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment hostEnv;
    private readonly IMemoryCache memCache;

    public MarkDownViewerMiddleware(RequestDelegate next,
        IWebHostEnvironment hostEnv, IMemoryCache memCache)
    {
        this.next = next;
        this.hostEnv = hostEnv;
        this.memCache = memCache;
    }

    /// <summary>
    /// 检测流的编码
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static string DetectCharset(Stream stream)
    {
        CharsetDetector charDetector = new();
        charDetector.Feed(stream);
        charDetector.DataEnd();
        string charset = charDetector.Charset ?? "UTF-8";
        stream.Position = 0;
        return charset;
    }

    /// <summary>
    /// 读取文本内容
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static async Task<string> ReadText(Stream stream)
    {
        string charset = DetectCharset(stream);
        using var reader = new StreamReader(stream, Encoding.GetEncoding(charset));
        return await reader.ReadToEndAsync();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path.Value ?? "";
        if (!path.EndsWith(".md"))
        {
            await next(context);
            return;
        }
        var file = hostEnv.WebRootFileProvider.GetFileInfo(path);
        if (!file.Exists)
        {
            await next(context);
            return;
        }
        context.Response.ContentType = $"text/html;charset=UTF-8";
        context.Response.StatusCode = 200;
        string cacheKey = nameof(MarkDownViewerMiddleware)
            + path + file.LastModified;
        var html = await memCache.GetOrCreateAsync(cacheKey, async ce => {
            ce.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            using var stream = file.CreateReadStream();
            string text = await ReadText(stream);
            Markdown markdown = new Markdown();
            return markdown.Transform(text);
        });
        await context.Response.WriteAsync(html);
    }
}