using Microsoft.EntityFrameworkCore;

namespace BgService1
{
    public class DemoBgService : BackgroundService
    {
        private ILogger<DemoBgService> logger;
        private readonly IServiceScope serviceScope;
        private readonly TestDbContext ctx;

        public DemoBgService(ILogger<DemoBgService> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.serviceScope = scopeFactory.CreateScope();
            var sp = serviceScope.ServiceProvider;
            this.ctx = sp.GetRequiredService<TestDbContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                int count = await ctx.Users.CountAsync();
                logger.LogInformation($"有{count}个用户");
                if(!File.Exists("d:/1.txt"))
                {
                    logger.LogError("d:/1.txt不存在");
                    return;
                }
                string s = await File.ReadAllTextAsync("d:/1.txt");
                logger.LogInformation($"读到了{s}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "未处理异常");
            }
            
        }

        public override void Dispose()
        {
            base.Dispose();
            serviceScope.Dispose();
        }

        /*
        private ILogger<DemoBgService> logger;

        public DemoBgService(ILogger<DemoBgService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000);
            logger.LogInformation("准备读文件");
            string s =await File.ReadAllTextAsync("d:/1.txt");
            await Task.Delay(5000);
            logger.LogInformation($"读到了{s}");
        }*/
    }
}
