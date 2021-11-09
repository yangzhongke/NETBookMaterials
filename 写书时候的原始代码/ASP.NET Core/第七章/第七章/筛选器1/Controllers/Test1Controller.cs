using BooksEFCore;
using Microsoft.AspNetCore.Mvc;

namespace 筛选器1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
        private readonly MyDbContext dbCtx;

        public Test1Controller(MyDbContext dbCtx)
        {
            this.dbCtx = dbCtx;
        }

        [HttpPost]
        public async Task Save()
        {
            dbCtx.Books.Add(new Book { Id=Guid.NewGuid(),Name="1",Price=1});
            await dbCtx.SaveChangesAsync();
            throw new Exception();
            dbCtx.Books.Add(new Book { Id = Guid.NewGuid(), Name = "2", Price = 2});
            await dbCtx.SaveChangesAsync();
        }


        [HttpGet]
        public string GetData()
        {
            /*
            string s = null;
            return s.ToUpper();*/
            Console.WriteLine("执行GetData");
            return "yzk";
        }

        [HttpGet]
        public ActionResult<string> GetName(int i)
        {
            if(i==0)
            {
                return "Zack";
            }
            else if(i==1)
            {
                return "杨中科";
            }
            else
            {
                return NotFound();
            }
        }
    }
}