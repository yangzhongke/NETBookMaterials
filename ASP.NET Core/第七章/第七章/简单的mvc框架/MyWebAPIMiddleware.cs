using System.Reflection;
using System.Text.Json;

namespace 简单的mvc框架
{
    public class MyWebAPIMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ActionLocator actionLocator;

        public MyWebAPIMiddleware(RequestDelegate next, ActionLocator actionLocator)
        {
            this.next = next;
            this.actionLocator = actionLocator;
        }

        //在运行的时候才能拿到能用的IServiceProvider
        //所以IServiceProvider不通过构造函数注入
        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            (bool ok,string? controllerName,string? actionName)=
                PathParser.Parse(context.Request.Path);
            if(ok==false)
            {
                await next(context);
                return;
            }
            MethodInfo? actionMethod = actionLocator.LocateActionMethod(controllerName!, actionName!);
            if(actionMethod ==null)
            {
                await next(context);
                return;
            }
            Type controllerType = actionMethod.DeclaringType!;
            object controllerInstance = serviceProvider.GetRequiredService(controllerType);
            var paraValues = BindingHelper.GetParameterValues(context, actionMethod);
            var result = actionMethod.Invoke(controllerInstance, paraValues);
            //限定返回值只能是普通类型，不能是IActionResult等
            string jsonStr = JsonSerializer.Serialize(result);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(jsonStr);
        }
    }
}
