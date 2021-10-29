using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zack.Commons
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
            var returnAssemblies = new HashSet<Assembly>();
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

            return returnAssemblies.ToArray();
        }
    }
}
