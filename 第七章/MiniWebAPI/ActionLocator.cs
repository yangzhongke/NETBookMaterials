using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MiniWebAPI
{
    public class ActionLocator
    {
        private Dictionary<string, MethodInfo> data = new(StringComparer.OrdinalIgnoreCase);
        private static bool IsControllerType(Type t)
        {
            return t.IsClass && !t.IsAbstract&&t.Name.EndsWith("Controller");
        }
        public ActionLocator(IServiceCollection services, Assembly assemblyWeb)
        {
            var controllerTypes = assemblyWeb.GetTypes().Where(IsControllerType);
            foreach (Type ctrlType in controllerTypes)
            {
                services.AddScoped(ctrlType);
                //去掉结尾的Controller
                int index = ctrlType.Name.LastIndexOf("Controller");
                string controllerName = ctrlType.Name.Substring(0, index);
                var methods = ctrlType.GetMethods(BindingFlags.Public|BindingFlags.Instance);
                foreach(var method in methods)
                {
                    string actionName = method.Name;
                    data[$"{controllerName}.{actionName}"] = method;
                }    
            }
        }

        public MethodInfo? LocateActionMethod(string controllerName,string actionName)
        {
            string key = $"{controllerName}.{actionName}";
            data.TryGetValue(key, out MethodInfo? method);
            return method;
        }
    }
}
