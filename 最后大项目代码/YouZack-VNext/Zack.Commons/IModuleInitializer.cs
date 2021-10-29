using Microsoft.Extensions.DependencyInjection;

namespace Zack.Commons
{
    /// <summary>
    /// 所有项目中的实现了IModuleInitializer接口都会被调用，请在Initialize中编写注册本模块需要的服务。
    /// 一个项目中可以放多个实现了IModuleInitializer的类。不过为了集中管理，还是建议一个项目中只放一个实现了IModuleInitializer的类
    /// </summary>
    public interface IModuleInitializer
    {
        public void Initialize(IServiceCollection services);
    }
}
