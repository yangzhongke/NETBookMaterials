using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace 模块化服务注册框架
{
    public static class ReflectionHelper
    {
        //是否是微软等的官方Assembly
        private static bool IsSystemAssembly(Assembly asm)
        {
            var asmCompanyAttr = asm.GetCustomAttribute<AssemblyCompanyAttribute>();
            if (asmCompanyAttr == null)
            {
                return false;
            }
            else
            {
                string companyName = asmCompanyAttr.Company;
                return companyName.Contains("Microsoft Corporation");
            }
        }

        /// <summary>
        /// loop through all assemblies
        /// 对于MSTest项目启动，Assembly.GetEntryAssembly()是"TestHost"，而"TestHost"的GetReferencedAssemblies
        /// 不包含项目的Dll，可能是因为"TestHost"都是通过Http调用被测试的程序集导致的，因此必须传入一个跟rootAssembly
        /// https://dotnetcoretutorials.com/2020/07/03/getting-assemblies-is-harder-than-you-think-in-c/
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAllReferencedAssemblies(Assembly rootAssembly, bool skipSystemAssemblies = true)
        {
            if (skipSystemAssemblies && IsSystemAssembly(rootAssembly))
            {
                throw new ArgumentOutOfRangeException(nameof(rootAssembly), "rootAssembly cannot be system-level assembly");
            }
            var returnAssemblies = new HashSet<Assembly>(new AssemblyEquality());
            var loadedAssemblies = new HashSet<string>();
            var assembliesToCheck = new Queue<Assembly>();

            assembliesToCheck.Enqueue(rootAssembly);
            returnAssemblies.Add(rootAssembly);

            while (assembliesToCheck.Any())
            {
                var assemblyToCheck = assembliesToCheck.Dequeue();

                foreach (var reference in assemblyToCheck.GetReferencedAssemblies())
                {
                    if (!loadedAssemblies.Contains(reference.FullName))
                    {
                        var assembly = Assembly.Load(reference);
                        if (skipSystemAssemblies && IsSystemAssembly(assembly))
                        {
                            continue;
                        }
                        assembliesToCheck.Enqueue(assembly);
                        loadedAssemblies.Add(reference.FullName);
                        returnAssemblies.Add(assembly);
                    }
                }
            }
            //如果一个程序集被引用了，但是我们没有直接用在代码中静态使用其中的类，
            //而是使用反射的形式使用了那些类，
            //那么我们是无法用GetReferencedAssemblies获得这个程序集的
            //因此这里通过扫描运行目录下的dll的形式再去补偿获取一些之前可能没扫描的程序集，
            //当然，在项目【发布】的时候，可能已经没有静态引用的程序集就没有被生成,
            //因此，如果遇到这种情况，就需要我们在代码中显式的静态引用那个程序集中的一个类了。
            var asmsInBaseDir = Directory.EnumerateFiles(AppContext.BaseDirectory,
                "*.dll", new EnumerationOptions { RecurseSubdirectories = true });
            foreach (var asmPath in asmsInBaseDir)
            {
                Assembly asm;
                try
                {
                    asm = Assembly.LoadFile(asmPath);
                }
                catch (BadImageFormatException)
                {
                    continue;
                }
                if (skipSystemAssemblies && IsSystemAssembly(asm))
                {
                    continue;
                }
                returnAssemblies.Add(asm);
            }
            return returnAssemblies.ToArray();
        }
        class AssemblyEquality : EqualityComparer<Assembly>
        {
            public override bool Equals(Assembly? x, Assembly? y)
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;
                return AssemblyName.ReferenceMatchesDefinition(x.GetName(), y.GetName());
            }

            public override int GetHashCode([DisallowNull] Assembly obj)
            {
                return obj.GetName().FullName.GetHashCode();
            }
        }
    }
}