using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using 例子服务接口1;
using 模块化服务注册框架;

ServiceCollection services = new ServiceCollection();
var asm = Assembly.GetExecutingAssembly();
ModuleHelper.RunModuleInitializers(services, asm);
using var sp = services.BuildServiceProvider();
var items = sp.GetServices<IMyService>();
foreach(var item in items)
{
    item.SayHello();
}