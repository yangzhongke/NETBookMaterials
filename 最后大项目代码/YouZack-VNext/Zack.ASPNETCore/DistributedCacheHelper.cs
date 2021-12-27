using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Zack.Commons;

namespace Zack.ASPNETCore
{
    public class DistributedCacheHelper : IDistributedCacheHelper
    {
        private readonly IDistributedCache distCache;

        public DistributedCacheHelper(IDistributedCache distCache)
        {
            this.distCache = distCache;
        }

        private static DistributedCacheEntryOptions CreateOptions(int baseExpireSeconds)
        {
            //过期时间.Random.Shared 是.NET6新增的
            double sec = Random.Shared.NextDouble(baseExpireSeconds, baseExpireSeconds * 2);
            TimeSpan expiration = TimeSpan.FromSeconds(sec);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = expiration;
            return options;
        }

        public TResult? GetOrCreate<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, TResult?> valueFactory, int expireSeconds = 60)
        {
            string jsonStr = distCache.GetString(cacheKey);
            //缓存中不存在
            if (string.IsNullOrEmpty(jsonStr))
            {
                var options = CreateOptions(expireSeconds);
                TResult? result = valueFactory(options);//如果数据源中也没有查到，可能会返回null
                //null会被json序列化为字符串"null"，所以可以防范“缓存穿透”
                string jsonOfResult = JsonSerializer.Serialize(result,
                    typeof(TResult));
                distCache.SetString(cacheKey, jsonOfResult, options);
                return result;
            }
            else
            {
                //"null"会被反序列化为null
                //TResult如果是引用类型，就有为null的可能性；如果TResult是值类型
                //在写入的时候肯定写入的是0、1之类的值，反序列化出来不会是null
                //所以如果obj这里为null，那么存进去的时候一定是引用类型
                distCache.Refresh(cacheKey);//刷新，以便于滑动过期时间延期
                return JsonSerializer.Deserialize<TResult>(jsonStr)!;
            }
        }

        public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, Task<TResult?>> valueFactory, int expireSeconds = 60)
        {
            string jsonStr = await distCache.GetStringAsync(cacheKey);
            if (string.IsNullOrEmpty(jsonStr))
            {
                var options = CreateOptions(expireSeconds);
                TResult? result = await valueFactory(options);
                string jsonOfResult = JsonSerializer.Serialize(result,
                    typeof(TResult));
                await distCache.SetStringAsync(cacheKey, jsonOfResult, options);
                return result;
            }
            else
            {
                await distCache.RefreshAsync(cacheKey);
                return JsonSerializer.Deserialize<TResult>(jsonStr)!;
            }
        }

        public void Remove(string cacheKey)
        {
            distCache.Remove(cacheKey);
        }

        public Task RemoveAsync(string cacheKey)
        {
            return distCache.RemoveAsync(cacheKey);
        }
    }
}
