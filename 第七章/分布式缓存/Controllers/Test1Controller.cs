using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Zack.ASPNETCore;

namespace 分布式缓存.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Test1Controller : ControllerBase
    {
		private readonly IDistributedCache distCache;
		private readonly IDistributedCacheHelper helper;
        public Test1Controller(IDistributedCache distCache, IDistributedCacheHelper helper)
        {
            this.distCache = distCache;
            this.helper = helper;
        }
        [HttpGet]
		public string Now()
		{
			string s = distCache.GetString("Now");
			if (s == null)
			{
				s = DateTime.Now.ToString();
				var opt = new DistributedCacheEntryOptions();
				opt.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
				distCache.SetString("Now", s, opt);
			}
			return s;
		}

		[HttpGet]
		public Task<string?> Now2()
        {
			return helper.GetOrCreateAsync<string>("Now2", async e => DateTime.Now.ToString());
        }

	}
}