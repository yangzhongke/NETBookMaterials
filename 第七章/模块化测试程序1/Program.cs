using Microsoft.Extensions.DependencyInjection;
using 例子服务接口1;
using Zack.Commons;

ServiceCollection services = new ServiceCollection();
var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
services.RunModuleInitializers(assemblies);
using var sp = services.BuildServiceProvider();
var items = sp.GetServices<IMyService>();
foreach(var item in items)
{
    item.SayHello();
}