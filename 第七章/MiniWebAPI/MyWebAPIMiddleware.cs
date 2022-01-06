using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace MiniWebAPI
{
    /**Web API的核心类
     * 对于MVC的控制器类做如下简单的约定:
     *1)控制器的类名以Controller结尾，除去结尾的Controller就是控制器的名字；
	 *2)控制器中所有的public方法就是控制器方法，方法的名字就是控制器的名字；
	 *3)请求路径固定为”/控制器名字/控制方法名字”；
	 *4)控制器的方法不支持重载，不支持通过[HttpGet]等方式绑定特定的Http谓词；
	 *5)控制器的方法可以没有参数，如果有参数就只支持一个参数。控制器方法的参数除非是HttpContext类型，否则请求报文体会按照Json格式进行反序列化；
	 *6)控制器的方法的返回值会被序列化为Json字符串，然后发送到响应报文中，不支持IActionResult等类型的返回值；
     */
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
        public async Task InvokeAsync(HttpContext context, IServiceProvider sp)
        {
            (bool ok,string? ctrlName,string? actionName)=PathParser.Parse(context.Request.Path);
            if(ok==false)
            {
                await next(context);
                return;
            }
            //使用控制器的名字和操作方法的名字来加载控制器方法对应的MethodInfo类型的对象
            var actionMethod = actionLocator.LocateActionMethod(ctrlName!, actionName!);
            if(actionMethod ==null)
            {
                await next(context);
                return;
            }
            Type controllerType = actionMethod.DeclaringType!;
            object controllerInstance = sp.GetRequiredService(controllerType);
            var paraValues = BindingHelper.GetParameterValues(context, actionMethod);
            foreach(var filter in ActionFilters.Filters)
            {
                filter.Execute();
            }
            var result = actionMethod.Invoke(controllerInstance, paraValues);
            //限定返回值只能是普通类型，不能是IActionResult等
            string jsonStr = JsonSerializer.Serialize(result);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(jsonStr);
        }
    }
}
