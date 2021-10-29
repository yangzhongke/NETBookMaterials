using LogServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleLogProviderExtensions
    {
        public static IServiceCollection AddConsoleLog(this IServiceCollection services)
        {
            return services.AddSingleton<ILogProvider, ConsoleLogProvider>();
        }
    }
}
