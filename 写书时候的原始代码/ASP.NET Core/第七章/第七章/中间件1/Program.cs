using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "中间件1", Version = "v1" });
});

var app = builder.Build();

/*//终段
app.Run(async ctx => {
    await ctx.Response.WriteAsync("cccc");
});*/
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "中间件1 v1"));
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//Map用于匹配请求（根据路径或者请求头等），然后组装中间件，用Use来定义中间件，一般把Run放到PipeLine的最后。
//UseMiddleWare则是使用Middleware class:Middleware class不需要继承任何类或是接口，但必须有名为Invoke，返回类型为Task，且第一个参数为HttpContext类型的方法。还需要有一个RequestDelegate next参数。
//map是新开一个“中间件请求路线分支”，在这个“分支”里，可以再用use和run方法来组件一个新的中间件逻辑。
//案例：实现一个简单的MVC框架，带auth、带缓存、带静态页、最后一个是404的中间件。
//或者不写这些案例，因为反正日常开发也用不到这些，简单的helloworld把几个组件的关系讲明白就行。重点讲一个服务器端缓存的Use就行了。
//案例：实现一个限流的use，和filter版对比。实现带缓存的use。中间件无法实现TransactionScopeFilter，因为无法读取Attribute
//案例：实现一个API Gateway(不做了，工作量太大)
app.Map("/testmap", async appbuilder  => {
    //注意Use的顺序
    appbuilder.Use(async (context, next) => {
        string pwd = context.Request.Query["password"];
        if(pwd=="123")
        {
            await next.Invoke();
        }
        else
        {
            context.Response.StatusCode = 401;
        }
    });
    appbuilder.Use(async (context, next) => {
        //微软的官方文档里要求中间件的使用要遵循如下规则：如response body改变后就不要再调用下一个中间件，避免下一个中间件对上一个中间件的httpcontext内容的污染。（本文示例为演示目的，未遵循此约定）
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
        await ctx.Response.WriteAsync("This is Map1<br/>");
    });
});
app.Run();
