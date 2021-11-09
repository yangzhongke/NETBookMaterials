using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System;
using System.Diagnostics;
using System.Security.Principal;

namespace Log2_95日志基础1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
services.AddLogging(logBuilder=> {
    logBuilder.AddConsole();
    //检查是否存在，不存在则创建
    if (!EventLog.SourceExists("mysource1"))
    {
        //需要管理员权限
        EventLog.CreateEventSource("mysource1", "MyCoreApp1");
    }
    logBuilder.AddEventLog(new EventLogSettings{ SourceName= "mysource1", LogName= "MyCoreApp1" });
});
            using (var sp = services.BuildServiceProvider())
            {
                //不能省略<Program>这个泛型
                var logger = sp.GetService<ILogger<Program>>();

                logger.LogInformation("这是普通信息");
                logger.LogWarning("这是一条警告消息");
                logger.LogError("这是一条错误消息");
                string age = "abc";
                logger.LogInformation("用户输入的年龄：{0}", age);
                try
                {
                    int i = int.Parse(age);
                    logger.LogInformation("年龄解析为:{0}", i);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "解析字符串为int失败");
                }
            }

            Console.ReadKey();
        }
    }
}