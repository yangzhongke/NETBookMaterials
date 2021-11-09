using Microsoft.AspNetCore.Mvc.Filters;

namespace 筛选器1
{
    public class MyActionFilter1 : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("MyActionFilter 1:开始执行");
            ActionExecutedContext r = await next();
            if(r.Exception != null)
            {
                Console.WriteLine("MyActionFilter 1:执行失败:"+r.Exception.Message);
            }
            else
            {
                Console.WriteLine("MyActionFilter 1:执行成功："+r.Result);
            }
        }
    }
}
