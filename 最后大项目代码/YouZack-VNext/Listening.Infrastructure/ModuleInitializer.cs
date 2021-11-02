using Listening.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace Listening.Domain
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IListeningRepository, ListeningRepository>();
        }
    }
}
