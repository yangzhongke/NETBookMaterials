using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;

namespace 筛选器1;
public class TransactionScopeFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool hasNotTransactionalAttribute = false;
        if (context.ActionDescriptor is ControllerActionDescriptor)
        {
            var actionDesc = (ControllerActionDescriptor)context.ActionDescriptor;
            hasNotTransactionalAttribute = actionDesc.MethodInfo
                .GetCustomAttributes(typeof(NotTransactionalAttribute), false).Any();
        }
        string requestMethod = context.HttpContext.Request.Method.ToUpper();
        if (hasNotTransactionalAttribute || requestMethod == "GET")
        //GET请求或者标注了[NotTransactional]则不启动事务
        {
            await next();
            return;
        }
        using (var txScope =
                new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = await next();
            //判断返回值得知是否发生了异常
            if (result.Exception == null)
            {
                txScope.Complete();
            }
        }
    }
}
