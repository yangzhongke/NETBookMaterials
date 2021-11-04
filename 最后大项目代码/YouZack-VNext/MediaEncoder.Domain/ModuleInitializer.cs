using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace MediaEncoder.Domain;
public class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<MediaEncoderFactory>();
    }
}
