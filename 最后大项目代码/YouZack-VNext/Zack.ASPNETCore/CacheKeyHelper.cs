using Microsoft.AspNetCore.Mvc;

namespace Zack.ASPNETCore
{
    public static class CacheKeyHelper
    {
        /// <summary>
        /// 获取和这个Action请求相关的CacheKey，主要考虑Controller名字、Action名字、参数等
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string CalcCacheKeyFromAction(this ControllerBase controller)
        {
            return GetCacheKey(controller.ControllerContext);
        }

        public static string GetCacheKey(this ControllerContext controllerContext)
        {
            var routeValues = controllerContext.RouteData.Values.Values;
            string cacheKey = string.Join(".", routeValues);
            return cacheKey;
        }
    }
}
