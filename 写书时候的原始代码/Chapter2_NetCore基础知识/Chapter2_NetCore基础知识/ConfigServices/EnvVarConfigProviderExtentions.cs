using ConfigServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EnvVarConfigProviderExtentions
    {
        public static IServiceCollection AddEnvVarConfig(this IServiceCollection services)
        {
            return services.AddSingleton<IConfigProvider, EnvVarConfigProvider>();
        }
    }
}