using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Threading.Tasks;
using SystemServices;

namespace Log2_97NLog
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddLogging(logBuilder => {
                logBuilder.AddNLog();
            });
            services.AddTransient<ReadFileService>();
            using (var sp = services.BuildServiceProvider())
            {
                //不能省略<Program>这个泛型
                var logger = sp.GetService<ILogger<Program>>();

                for(int i=0;i<20000;i++)
                {
                    logger.LogInformation("这是普通信息");
                    logger.LogWarning("这是一条警告消息");
                    logger.LogError("这是一条错误消息");
                    ReadFileService rfService = sp.GetService<ReadFileService>();
                    await rfService.Read("d:/1.txt");
                }
                System.Console.WriteLine("结束");
            }
        }
    }
}