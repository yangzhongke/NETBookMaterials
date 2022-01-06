using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

[Route("[controller]/[action]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    private readonly ILogger<Test1Controller> logger;
    private readonly MyDbContext dbCtx;
    private readonly IMemoryCache memCache;
    public Test1Controller(MyDbContext dbCtx, IMemoryCache memCache, ILogger<Test1Controller> logger)
    {
        this.dbCtx = dbCtx;
        this.memCache = memCache;
        this.logger = logger;
    }
    [HttpGet]
    public async Task<Book[]> GetBooks()
    {
        logger.LogInformation("开始执行GetBooks");
        var items = await memCache.GetOrCreateAsync("AllBooks", async (e) =>
        {
            logger.LogInformation("从数据库中读取数据");
            return await dbCtx.Books.ToArrayAsync();
        });
        logger.LogInformation("把数据返回给调用者");
        return items;
    }

    
    [HttpGet]
    public async Task<Book[]> Demo1()
    {
        //绝对过期时间
        /*
        logger.LogInformation("开始执行Demo1：" + DateTime.Now);
        var items = await memCache.GetOrCreateAsync("AllBooks", async (e) => {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
            logger.LogInformation("从数据库中读取数据");
            return await dbCtx.Books.ToArrayAsync();
        });
        logger.LogInformation("Demo1执行结束");*/
        //滑动过期时间
        
        logger.LogInformation("开始执行Demo2：" + DateTime.Now);
        var items = await memCache.GetOrCreateAsync("AllBooks2", async (e) => {
            e.SlidingExpiration = TimeSpan.FromSeconds(10);
            logger.LogInformation("Demo2从数据库中读取数据");
            return await dbCtx.Books.ToArrayAsync();
        });
        logger.LogInformation("Demo2执行结束");
        
        //混合使用过期时间策略
        /*
        logger.LogInformation("开始执行Demo3：" + DateTime.Now);
        var items = await memCache.GetOrCreateAsync("AllBooks3", async (e) => {
            e.SlidingExpiration = TimeSpan.FromSeconds(10);
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
            logger.LogInformation("Demo3从数据库中读取数据");
            return await dbCtx.Books.ToArrayAsync();
        });
        logger.LogInformation("Demo3执行结束");
        */
        return items;
    }

    [HttpGet]
    public async Task<ActionResult<Book?>> Demo5(Guid id)
    {
        /*
        string cacheKey = "Book" + id;//缓存键
        Book? b = memCache.Get<Book?>(cacheKey);
        if (b == null)//如果缓存中没有数据
        {
            //查询数据库，然后写入缓存
            b = await dbCtx.Books.FindAsync(id);
            memCache.Set(cacheKey, b);
        }
        if (b == null)//如果仍然没找到数据
            return NotFound("找不到这本书");
        else
            return b;
        */
        logger.LogInformation("开始执行Demo5");
        string cacheKey = "Book" + id;
        var book = await memCache.GetOrCreateAsync(cacheKey, async (e) => {
            var b = await dbCtx.Books.FindAsync(id);
            logger.LogInformation("数据库查询：{0}", b == null ? "为空" : "不为空");
            return b;
        });
        logger.LogInformation("Demo5执行结束:{0}", book == null ? "为空" : "不为空");
        return book;

    }
}
