using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Zack.ASPNETCore
{
    public interface IMemoryCacheHelper
    {
        /// <summary>
        /// 从缓存中获取数据，如果缓存中没有数据，则调用valueFactory获取数据。
        /// 可以用AOP+Attribute的方式来修饰到Service接口中实现缓存，更加优美，但是没有这种方式更灵活。
        /// 默认最长的缓存过期时间是expireSeconds秒，当然也可以在领域事件的Handler中调用Update更新缓存，或者调用Remove删除缓存。
        /// 因为IMemoryCache会把null当成合法的值，因此不会有缓存穿透的问题，但是还是建议用我这里封装的ICacheHelper，原因如下：
        /// 1）可以切换别的实现类，比如可以保存到MemCached、Redis等地方。这样可以隔离变化。
        /// 2）IMemoryCache的valueFactory用起来麻烦，还要单独声明一个ICacheEntry参数，大部分时间用不到这个参数。
        /// 3）这里把expireSeconds加上了一个随机偏差，这样可以避免短时间内同样的请求集中过期导致“缓存雪崩”的问题
        /// 4）这里加入了缓存数据的类型不能是IEnumerable、IQueryable等类型的限制
        /// </summary>
        /// <typeparam name="TResult">缓存的值的类型</typeparam>
        /// <param name="cacheKey">缓存的key</param>
        /// <param name="valueFactory">提供数据的委托</param>
        /// <param name="expireSeconds">缓存过期秒数的最大值，实际缓存时间是在[expireSeconds,expireSeconds*2)之间，这样可以一定程度上避免大批key集中过期导致的“缓存雪崩”的问题</param>
        /// <returns></returns>
        TResult? GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult?> valueFactory, int expireSeconds = 60);

        Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<ICacheEntry, Task<TResult?>> valueFactory, int expireSeconds = 60);

        /// <summary>
        /// 删除缓存的值
        /// </summary>
        /// <param name="cacheKey"></param>
        void Remove(string cacheKey);
    }
}
