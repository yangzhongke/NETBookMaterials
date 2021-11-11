using Microsoft.Extensions.DependencyInjection;

namespace 模块化的服务注册;
public interface IModuleInitializer
{
	public void Initialize(IServiceCollection services);
}