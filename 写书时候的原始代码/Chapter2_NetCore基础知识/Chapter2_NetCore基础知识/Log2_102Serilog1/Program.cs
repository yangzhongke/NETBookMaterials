using Exceptionless;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using SystemServices;

namespace Log2_102Serilog1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            ExceptionlessClient.Default.Startup("T36FuRqxCX6qvoH8I6u4osMhmgHn5IR82uH6cyJb");

            ServiceCollection services = new ServiceCollection();
            services.AddLogging(builder => {
                Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Debug()
                   .Enrich.FromLogContext()
                   .WriteTo.Exceptionless()
                   .WriteTo.Console(new JsonFormatter())
                   .CreateLogger();
                builder.AddSerilog();
                builder.AddErrorMail(opt => {
                    opt.From = "";
                });
            });
            services.AddTransient<ReadFileService>();
            using(var sp = services.BuildServiceProvider())
            {
                var logger = sp.GetService<ILogger<Program>>();
                logger.LogWarning("新增用户 {@person}",new { Id=3,Name="zack"});
                ReadFileService rfService = sp.GetService<ReadFileService>();
                await rfService.Read("d:/1.txt");
            }*/

ServiceCollection services = new ServiceCollection();
services.AddLogging(builder => {
    builder.AddErrorMail(opt => {
        opt.IntervalSeconds = 30;
        opt.SendSameErrorOnlyOnce = true;
        opt.From = "tianpo@rupeng.com";
        opt.To = new []{"yzk@rupeng.com" };
        opt.SmtpEnableSsl = true;
        opt.SmtpUserName = "tianpo@rupeng.com";
        opt.SmtpPassword = "4bNxj7C9y6";
        opt.SmtpServer = "smtp.ym.163.com";
        opt.SmtpEnableSsl = true;                    
    });
    builder.AddConsole();
});
using (var sp = services.BuildServiceProvider())
{                
    for(int i=0;i<100;i++)
    {
        using (var scope = sp.CreateScope())
        {
            var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
            string s = "abc";
            try
            {
                int.Parse(s);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "解析错误,s={0}", s);
            }
        }
        Thread.Sleep(500);
    }                            
}
        }
    }
}