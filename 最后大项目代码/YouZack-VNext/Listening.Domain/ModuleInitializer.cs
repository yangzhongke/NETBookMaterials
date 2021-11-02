using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace Listening.Domain
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ListeningDomainService>();
        }
    }
}
