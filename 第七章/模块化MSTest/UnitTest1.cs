using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;
using 例子服务接口1;
using 模块化服务注册框架;

namespace 模块化MSTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ServiceCollection services = new ServiceCollection();
            ModuleHelper.RunModuleInitializers(services);
            using var sp = services.BuildServiceProvider();
            var items = sp.GetServices<IMyService>();
            Assert.AreEqual(items.Count(), 2);
        }
    }
}