using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zack.Commons;

namespace Zack.ASPNETCore
{
    /// <summary>
    /// 用ASP.NET的IMemoryCache实现的内存缓存
    /// </summary>
    public class MemoryCacheHelper : IMemoryCacheHelper
    {
        private readonly IMemoryCache memoryCache;
        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        private static void ValidateValueType<TResult>()
        {
            //因为IEnumerable、IQueryable等有延迟执行的问题，造成麻烦，因此禁止用这些类型
            Type typeResult = typeof(TResult);
            if (typeResult.IsGenericType)//如果是IEnumerable<String>这样的泛型类型，则把String这样的具体类型信息去掉，再比较
            {
                typeResult = typeResult.GetGenericTypeDefinition();
            }
            //注意用相等比较，不要用IsAssignableTo
            if (typeResult == typeof(IEnumerable<>) || typeResult == typeof(IEnumerable)
                || typeResult == typeof(IAsyncEnumerable<TResult>)
                || typeResult == typeof(IQueryable<TResult>) || typeResult == typeof(IQueryable))
            {
                throw new InvalidOperationException($"TResult of {typeResult} is not allowed, please use List<T> or T[] instead.");
            }
        }

        private static void InitCacheEntry(ICacheEntry entry, int baseExpireSeconds)
        {
            //过期时间.Random.Shared 是.NET6新增的
            double sec = Random.Shared.NextDouble(baseExpireSeconds, baseExpireSeconds * 2);
            TimeSpan expiration = TimeSpan.FromSeconds(sec);
            entry.AbsoluteExpirationRelativeToNow = expiration;
        }

        public TResult? GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult?> valueFactory, int baseExpireSeconds = 60)
        {
            ValidateValueType<TResult>();
            //因为IMemoryCache保存的是一个CacheEntry，所以null值也认为是合法的，因此返回null不会有“缓存穿透”的问题
            //不调用系统内置的CacheExtensions.GetOrCreate，而是直接用GetOrCreate的代码，这样免得包装一次委托
            if (!memoryCache.TryGetValue(cacheKey, out TResult result))
            {
                using ICacheEntry entry = memoryCache.CreateEntry(cacheKey);
                InitCacheEntry(entry, baseExpireSeconds);
                result = valueFactory(entry)!;
                entry.Value = result;
            }
            return result;
        }

        public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<ICacheEntry, Task<TResult?>> valueFactory, int baseExpireSeconds = 60)
        {
            ValidateValueType<TResult>();
            if (!memoryCache.TryGetValue(cacheKey, out TResult result))
            {
                using ICacheEntry entry = memoryCache.CreateEntry(cacheKey);
                InitCacheEntry(entry, baseExpireSeconds);
                result = (await valueFactory(entry))!;
                entry.Value = result;
            }
            return result;
        }

        public void Remove(string cacheKey)
        {
            memoryCache.Remove(cacheKey);
        }
    }
}
