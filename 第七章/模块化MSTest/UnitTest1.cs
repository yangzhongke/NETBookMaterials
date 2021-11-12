using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Zack.Commons;
using 例子服务接口1;

namespace 模块化MSTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            ServiceCollection services = new ServiceCollection();
            services.RunModuleInitializers(assemblies);
            using var sp = services.BuildServiceProvider();
            var items = sp.GetServices<IMyService>();
            Assert.AreEqual(items.Count(), 2);
        }
    }
}