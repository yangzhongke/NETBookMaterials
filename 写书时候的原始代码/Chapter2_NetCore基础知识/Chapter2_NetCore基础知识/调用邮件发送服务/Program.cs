using MailServices;
using Microsoft.Extensions.DependencyInjection;

namespace 调用邮件发送服务
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddConsoleLog();
            services.AddIniFileConfig("d:/mail.ini");
            services.AddEnvVarConfig();
            services.AddLayeredConfigReader();
            services.AddDefaultMailSender();
            using(var sp = services.BuildServiceProvider())
            {
                var mailSender = sp.GetService<IMailSender>();
                mailSender.Send("abc@test.com", "hello", "Kia ora!");
            }
        }
    }
}
