using Microsoft.Extensions.Caching.Memory;

namespace BgService1
{
    public class LoadUsersCacheBgService : BackgroundService
    {
        private readonly TestDbContext ctx;
        private readonly ILogger<ExplortStatisticBgService> logger;
        private readonly IServiceScope serviceScope;
        private readonly IMemoryCache memCache;
        /*
        public LoadUsersCacheBgService(IServiceScopeFactory scopeFactory)*/
        //InvalidOperationException: Cannot consume scoped service 'BgService1.TestDbContext' from singleton 'Microsoft.Extensions.Hosting.IHostedService'.
        //TestDbContext不可以直接注入，IMemoryCache、ILogger都可以
        public LoadUsersCacheBgService(IServiceScopeFactory scopeFactory, ILogger<ExplortStatisticBgService> logger,
             IMemoryCache memCache)
        {
            //非Scope服务可以直接构造函数注入（比如ILogger），可是查是不是Scope很麻烦，所以感觉都统一通过IServiceScopeFactory获取
            this.serviceScope = scopeFactory.CreateScope();
            var sp = serviceScope.ServiceProvider;
            /*
            
            this.logger = sp.GetRequiredService<ILogger<ExplortStatisticBgService>>();
            this.memCache = sp.GetRequiredService<IMemoryCache>();*/
            this.ctx = sp.GetRequiredService<TestDbContext>();
            this.logger = logger;
            this.memCache = memCache;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                foreach(var u in ctx.Users)
                {
                    memCache.Set($"UserCache.{u.Id}", u);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "出现未处理异常");
            }
            return Task.CompletedTask;
        }
    }
}
