using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using 模块化的服务注册;

namespace 模块化服务注册框架;

public static class ModuleHelper
{
	public static IServiceCollection RunModuleInitializers(this IServiceCollection services, Type rootType)
	{
		return RunModuleInitializers(services, rootType.Assembly);
	}

	public static IServiceCollection RunModuleInitializers(this IServiceCollection services, Assembly rootAssembly)
	{
		var asms = ReflectionHelper.GetAllReferencedAssemblies(rootAssembly);
		return RunModuleInitializers(services, asms);
	}

	public static IServiceCollection RunModuleInitializers(this IServiceCollection services, IEnumerable<Assembly> assemblies)
	{
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

