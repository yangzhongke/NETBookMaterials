using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zack.Commons;

namespace Microsoft.EntityFrameworkCore
{
    public static class EFCoreInitializerHelper
    {
        /// <summary>
        /// 自动为所有的DbContext注册连接配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        public static IServiceCollection AddAllDbContexts(this IServiceCollection services, Action<DbContextOptionsBuilder> builder,
            IEnumerable<Assembly> assemblies)
        {
            //AddDbContextPool不支持DbContext注入其他对象，而且使用不当有内存暴涨的问题，因此不用AddDbContextPool
            Type[] types = new Type[] { typeof(IServiceCollection), typeof(Action<DbContextOptionsBuilder>), typeof(ServiceLifetime), typeof(ServiceLifetime) };
            var methodAddDbContext = typeof(EntityFrameworkServiceCollectionExtensions)
                .GetMethod(nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext), 1, types);
            foreach (var asmToLoad in assemblies)
            {
                Type[] typesInAsm = asmToLoad.GetTypes();
                //Register DbContext
                //GetTypes() include public/protected ones
                //GetExportedTypes only include public ones
                //so that XXDbContext in Agrregation can be internal to keep insulated
                foreach (var dbCtxType in typesInAsm
                    .Where(t => !t.IsAbstract && typeof(DbContext).IsAssignableFrom(t)))
                {
                    //similar to serviceCollection.AddDbContextPool<ECDictDbContext>(opt=>new DbContextOptionsBuilder(dbCtxOpt));
                    var methodGenericAddDbContext = methodAddDbContext.MakeGenericMethod(dbCtxType);
                    methodGenericAddDbContext.Invoke(null, new object[] { services, builder, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
                }
            }
            return services;
        }

    }
}
