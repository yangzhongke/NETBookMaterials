using 空项目起点的中间件1;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Map("/test", async appbuilder => {
    appbuilder.UseMiddleware<CheckAndParsingMiddleware>();
    appbuilder.Run(async ctx => {
        ctx.Response.ContentType = "text/html";
        ctx.Response.StatusCode = 200;
        dynamic? jsonObj = ctx.Items["BodyJson"];
        int i = jsonObj.i;
        int j = jsonObj.j;
        await ctx.Response.WriteAsync($"{i}+{j}={i+j}");
    });
});
app.MapWhen(ctx=>ctx.Request.Headers["AAA"]=="123", async appbuilder => { });
app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), async appbuilder => { });

app.Run();
/*
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
        await ctx.Response.WriteAsync("hello middleware<br/>");
    });
});
app.Run();
*/