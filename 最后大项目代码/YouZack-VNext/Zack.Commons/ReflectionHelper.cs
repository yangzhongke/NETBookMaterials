using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace Zack.Commons;
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
            return companyName.Contains("Microsoft");
        }
    }

    /// <summary>
    /// 判断file这个文件是否是程序集
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private static bool IsManagedAssembly(string file)
    {
        using var fs = File.OpenRead(file);
        using PEReader peReader = new PEReader(fs);
        var peHeaders = peReader.PEHeaders;
        return peHeaders.CorHeader != null;
        /*
        using var fs1 = File.OpenRead(@"E:\主同步盘\我的坚果云\个人资料\写书\Net Core书稿\随书代码\最后大项目代码\YouZack-VNext\Listening.Admin.WebAPI\bin\Debug\net6.0\runtimes\win-arm\native\Microsoft.Data.SqlClient.SNI.dll");
        using PEReader peReader1 = new PEReader(fs1);
        var peHeaders1 = peReader1.PEHeaders;
        using var fs2 = File.OpenRead(@"E:\主同步盘\我的坚果云\个人资料\写书\Net Core书稿\随书代码\最后大项目代码\YouZack-VNext\Listening.Admin.WebAPI\bin\Debug\net6.0\System.Data.SqlClient.dll");
        using PEReader peReader2 = new PEReader(fs2);
        var peHeaders2 = peReader2.PEHeaders;
        string s1 = peHeaders1.ToJsonString();
        string s2 = peHeaders2.ToJsonString();*/
        //return true;
    }

    /// <summary>
    /// loop through all assemblies
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetAllReferencedAssemblies(bool skipSystemAssemblies = true)
    {
        /// 对于MSTest项目启动，Assembly.GetEntryAssembly()是"TestHost"，而"TestHost"的GetReferencedAssemblies
        /// 不包含项目的Dll，可能是因为"TestHost"都是通过Http调用被测试的程序集导致的，因此必须传入一个跟rootAssembly
        /// https://dotnetcoretutorials.com/2020/07/03/getting-assemblies-is-harder-than-you-think-in-c/
        Assembly? rootAssembly = Assembly.GetEntryAssembly();
        if (rootAssembly == null)
        {
            rootAssembly = Assembly.GetCallingAssembly();
        }
        var returnAssemblies = new HashSet<Assembly>(new AssemblyEquality());
        var loadedAssemblies = new HashSet<string>();
        var assembliesToCheck = new Queue<Assembly>();
        assembliesToCheck.Enqueue(rootAssembly);
        if (skipSystemAssemblies && IsSystemAssembly(rootAssembly) != false)
        {
            returnAssemblies.Add(rootAssembly);
        }
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
        //以下代码解决两个问题：
        //1、如果一个程序集被引用了，但是我们没有直接用在代码中静态使用其中的类，
        //而是使用反射的形式使用了那些类，
        //那么我们是无法用GetReferencedAssemblies获得这个程序集的
        //2、以MsTest运行的时候GetEntryAssembly()拿到的是TestHost，从而无法进一步的获取它引用的程序集。
        //"TestHost"的GetReferencedAssemblies不包含项目的Dll，可能是因为"TestHost"
        //都是通过Http调用被测试的程序集导致的。
        //https://dotnetcoretutorials.com/2020/07/03/getting-assemblies-is-harder-than-you-think-in-c/
        //因此这里通过扫描运行目录下的dll的形式再去补偿获取一些之前可能没扫描的程序集，
        //当然，在项目【发布】的时候，可能已经没有静态引用的程序集就没有被生成,
        //因此，如果遇到这种情况，就需要我们在代码中显式的静态引用那个程序集中的一个类了。
        var asmsInBaseDir = Directory.EnumerateFiles(AppContext.BaseDirectory,
            "*.dll", new EnumerationOptions { RecurseSubdirectories = true });
        foreach (var asmPath in asmsInBaseDir)
        {
            if(!IsManagedAssembly(asmPath))
            {
                continue;
            }
            AssemblyName asmName = AssemblyName.GetAssemblyName(asmPath);
            //如果程序集已经加载过了就不再加载
            if(returnAssemblies.Any(x=>AssemblyName.ReferenceMatchesDefinition(x.GetName(),asmName)))
            {
                continue;
            }
            Assembly asm = Assembly.Load(asmName);
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