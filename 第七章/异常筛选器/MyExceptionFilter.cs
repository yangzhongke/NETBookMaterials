using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class MyExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<MyExceptionFilter> logger;
    private readonly IHostEnvironment env;
    public MyExceptionFilter(ILogger<MyExceptionFilter> logger, IHostEnvironment env)
    {
        this.logger = logger;
        this.env = env;
    }
    public Task OnExceptionAsync(ExceptionContext context)
    {
        Exception exception = context.Exception;
        logger.LogError(exception, "UnhandledException occured");
        string message;
        if (env.IsDevelopment())
        {
            message = exception.ToString();
        }
        else
        {
            message = "程序中出现未处理异常";
        }
        ObjectResult result = new ObjectResult(new { code = 500, message = message });
        result.StatusCode = 500;
        context.Result = result;
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}