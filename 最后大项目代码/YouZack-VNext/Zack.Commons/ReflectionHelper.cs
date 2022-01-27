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
    }

    /// <summary>
    /// loop through all assemblies
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetAllReferencedAssemblies(bool skipSystemAssemblies = true)
    {
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
            if (IsValid(rootAssembly))
            {
                returnAssemblies.Add(rootAssembly);
            }
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
                    if (IsValid(assembly))
                    {
                        returnAssemblies.Add(assembly);
                    }
                }
            }
        }
        var asmsInBaseDir = Directory.EnumerateFiles(AppContext.BaseDirectory,
            "*.dll", new EnumerationOptions { RecurseSubdirectories = true });
        foreach (var asmPath in asmsInBaseDir)
        {
            if (!IsManagedAssembly(asmPath))
            {
                continue;
            }
            AssemblyName asmName = AssemblyName.GetAssemblyName(asmPath);
            //如果程序集已经加载过了就不再加载
            if (returnAssemblies.Any(x => AssemblyName.ReferenceMatchesDefinition(x.GetName(), asmName)))
            {
                continue;
            }
            Assembly asm = Assembly.LoadFile(asmPath);
            //Assembly asm = Assembly.Load(asmName);
            if (!IsValid(asm))
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

    private static bool IsValid(Assembly asm)
    {
        try
        {
            asm.GetTypes();
            asm.DefinedTypes.ToList();
            return true;
        }
        catch (ReflectionTypeLoadException)
        {
            return false;
        }
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