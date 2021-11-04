using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zack.Commons
{
    public static class ModuleInitializerExtensions
    {
        public static IServiceCollection RunModuleInitializers(this IServiceCollection services, Type rootType)
        {
            return RunModuleInitializers(services, rootType.Assembly);
        }
        /// <summary>
        /// 运行rootAssembly及直接或者间接引用的程序集（排除系统程序集）中的IModuleInitializer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="rootAssembly"></param>
        /// <returns></returns>
        public static IServiceCollection RunModuleInitializers(this IServiceCollection services, Assembly rootAssembly)
        {
            //todo：这里有bug：如果一个程序集被引用了，但是没有使用其中的类，
            ////则这里asms里拿不到那个程序集
            ///可能的解决思路：用AppContext.BaseDirectory再扫描一遍dll，再补偿获取一遍
            var asms = ReflectionHelper.GetAllReferencedAssemblies(rootAssembly);
            return RunModuleInitializers(services, asms);
        }

        /// <summary>
        /// 每个项目中都可以自己写一些实现了IModuleInitializer接口的类，在其中注册自己需要的服务，这样避免所有内容到入口项目中注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        public static IServiceCollection RunModuleInitializers(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            foreach (var implType in assemblies.SelectMany(asm => asm.GetTypes())
                .Where(t => !t.IsAbstract && typeof(IModuleInitializer).IsAssignableFrom(t)))
            {
                IModuleInitializer? initializer = (IModuleInitializer?)Activator.CreateInstance(implType);
                if(initializer == null)
                {
                    throw new ApplicationException("Cannot create an instance of"+implType);
                }
                initializer.Initialize(services);
            }
            return services;
        }
    }
}
