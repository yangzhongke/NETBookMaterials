using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace Zack.JWT
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
