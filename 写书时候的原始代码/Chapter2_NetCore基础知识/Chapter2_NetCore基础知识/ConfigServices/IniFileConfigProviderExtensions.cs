using ConfigServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IniFileConfigProviderExtensions
    {
        public static IServiceCollection AddIniFileConfig(this IServiceCollection services,string filePath)
        {
            return services.AddSingleton<IConfigProvider>(sp => {
                IniFileConfigProvider iniService = new IniFileConfigProvider();
                iniService.FilePath = filePath;
                return iniService;
            });
        }
    }
}