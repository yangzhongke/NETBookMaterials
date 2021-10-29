using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Zack.ASPNETCore
{
    /// <summary>
    ///GET请求以及标注了NotTransactionalAttribute的Controller方法不启动事务，其他请求都自动启动TransactionScope事务
    //因为EF Core的SaveChanges会自动启动事务，所以如果全部都是EFCore的操作，不需要开发者手写BeginTransaction
    //TransactionScope也能实现嵌入事务
    //因为一个普通的操作生成的领域事件可能
    /// </summary>
    public class TransactionScopeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasNotTransactionalAttribute = false;
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                ControllerActionDescriptor actionDesc = (ControllerActionDescriptor)context.ActionDescriptor;
                if (actionDesc.MethodInfo.GetCustomAttributes(typeof(NotTransactionalAttribute), false).Any())
                {
                    hasNotTransactionalAttribute = true;
                }
            }
            string requestMethod = context.HttpContext.Request.Method.ToUpper();
            if (hasNotTransactionalAttribute || requestMethod == "GET")//如果是GET请求或者Action方法标注了[NotTransactional]则不启动事务
            {
                await next();
            }
            else
            {
                using (var txScope =
                    new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = await next();
                    //Action中发生异常的时候await next()不会报异常，所以需要判断返回值得知是否发生了异常
                    if (result.Exception == null)
                    {
                        txScope.Complete();
                    }
                }
            }
        }
    }
}
