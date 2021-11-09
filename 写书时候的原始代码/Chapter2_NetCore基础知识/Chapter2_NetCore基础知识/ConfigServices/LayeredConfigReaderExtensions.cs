using ConfigServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LayeredConfigReaderExtensions
    {
        public static IServiceCollection AddLayeredConfigReader(this IServiceCollection services)
        {
            return services.AddSingleton<IConfigReader, LayeredConfigReader>();
        }
    }
}