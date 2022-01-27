using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zack.Commons;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ModuleInitializerExtensions
    {
		/// <summary>
		/// 每个项目中都可以自己写一些实现了IModuleInitializer接口的类，在其中注册自己需要的服务，这样避免所有内容到入口项目中注册
		/// </summary>
		/// <param name="services"></param>
		/// <param name="assemblies"></param>
		public static IServiceCollection RunModuleInitializers(this IServiceCollection services,
		 IEnumerable<Assembly> assemblies)
		{
			foreach(var asm in assemblies)
            {
				Type[] types = asm.GetTypes();
				var moduleInitializerTypes = types.Where(t => !t.IsAbstract && typeof(IModuleInitializer).IsAssignableFrom(t));
				foreach(var implType in moduleInitializerTypes)
                {
					var initializer = (IModuleInitializer?)Activator.CreateInstance(implType);
					if (initializer == null)
					{
						throw new ApplicationException($"Cannot create ${implType}");
					}
					initializer.Initialize(services);
				}
			}
			return services;
		}
	}
}
