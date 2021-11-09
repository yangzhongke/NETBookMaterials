using BooksEFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace 分布式缓存1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
        private readonly MyDbContext dbCtx;
        private readonly IDistributedCache distCache;
        private readonly IDistributedCacheHelper distCacheHelper;
        public Test1Controller(MyDbContext dbCtx, IDistributedCache distCache, IDistributedCacheHelper distCacheHelper)
        {
            this.dbCtx = dbCtx;
            this.distCache = distCache;
            this.distCacheHelper = distCacheHelper;
        }

        [HttpGet]
        public string Now()
        {
            string s = distCache.GetString("Now");
            if(s==null)
            {
                s = DateTime.Now.ToString();
                var opt = new DistributedCacheEntryOptions();
                opt.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                distCache.SetString("Now", s, opt);
            }
            return s;
        }

        [HttpGet]
        public async Task<Book[]> GetBooks()
        {            
            var books = await distCacheHelper.GetOrCreateAsync("Books", async (e) => await dbCtx.Books.ToArrayAsync(),60);           
            return books;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Book> GetBook(Guid id)
        {
            var b = await distCacheHelper.GetOrCreateAsync("Book"+id, async (e) => await dbCtx.Books.FindAsync(id), 30);
            return b;
        }
    }
}