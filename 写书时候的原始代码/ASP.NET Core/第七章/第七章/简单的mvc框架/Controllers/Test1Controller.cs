using Microsoft.Extensions.Caching.Memory;
using 简单的mvc框架.Models;

namespace abc
{
    public class Test1Controller
	{
		private IMemoryCache memoryCache;
		public Test1Controller(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}
		public Person IncAge(Person p)
		{
			p.Age++;
			return p;
		}
		public object[] Get2(HttpContext ctx)
		{
			DateTime now = memoryCache.GetOrCreate("Now", e => DateTime.Now);
			string name = ctx.Request.Query["name"];
			return new object[] { "hello" + name, now };
		}
	}
}
