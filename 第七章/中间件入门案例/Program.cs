var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Map("/test", async appbuilder => {
    appbuilder.Use(async (context, next) => {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("1  Start<br/>");
        await next.Invoke();
        await context.Response.WriteAsync("1  End<br/>");
    });
    appbuilder.Use(async (context, next) => {
        await context.Response.WriteAsync("2  Start<br/>");
        await next.Invoke();
        await context.Response.WriteAsync("2  End<br/>");
    });
    appbuilder.Run(async ctx => {
        await ctx.Response.WriteAsync("hello middleware <br/>");
    });
});
app.Run();