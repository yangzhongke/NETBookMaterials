using Microsoft.Extensions.DependencyInjection;
using 例子服务实现1;
using 例子服务接口1;
using 模块化的服务注册;
class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IMyService, CnService>();
    }
}