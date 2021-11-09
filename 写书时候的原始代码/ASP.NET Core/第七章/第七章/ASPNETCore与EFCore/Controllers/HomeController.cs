using BooksEFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ASPNETCore与EFCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext dbCtx;

        public HomeController(MyDbContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        public async Task<IActionResult> Index()
        {
            dbCtx.Add(new Book { Id=Guid.NewGuid(),Name="零基础趣学C语言",Price=59});
            await dbCtx.SaveChangesAsync();
            var book = dbCtx.Books.First();
            return Content(book.ToString()); ;
        }
    }
}