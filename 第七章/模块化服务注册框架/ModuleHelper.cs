using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;
using 模块化的服务注册;

namespace 模块化服务注册框架;

public static class ModuleHelper
{
	public static IServiceCollection RunModuleInitializers(this IServiceCollection services)
	{
		var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
		foreach (var implType in assemblies.SelectMany(asm => asm.GetTypes())
			.Where(t => !t.IsAbstract && typeof(IModuleInitializer).IsAssignableFrom(t)))
		{
			IModuleInitializer? initializer = (IModuleInitializer?)Activator.CreateInstance(implType);
			if (initializer == null)
			{
				throw new ApplicationException("Cannot create an instance of" + implType);
			}
			initializer.Initialize(services);
		}
		return services;
	}
}

