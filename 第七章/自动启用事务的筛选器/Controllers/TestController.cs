using Microsoft.AspNetCore.Mvc;

namespace 自动启用事务的筛选器.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly MyDbContext dbCtx;

        public TestController(MyDbContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        [HttpPost]
        public async Task Save()
        {
            dbCtx.Books.Add(new Book { Id = Guid.NewGuid(), Name = "1", Price = 1 });
            await dbCtx.SaveChangesAsync();
            dbCtx.Books.Add(new Book { Id = Guid.NewGuid(), Name = "2", Price = 2 });
            await dbCtx.SaveChangesAsync();
            throw new Exception();
        }

    }
}