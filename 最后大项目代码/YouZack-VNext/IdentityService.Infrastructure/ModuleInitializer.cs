using IdentityService.Domain;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace IdentityService.Infrastructure
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IdDomainService>();
            services.AddScoped<IIdRepository, IdRepository>();
        }
    }
}