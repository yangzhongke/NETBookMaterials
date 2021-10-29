using FileService.Domain.Services;
using FileService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Zack.Commons;

namespace FileService.Infrastructure
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IStorageClient, SMBStorageClient>();
            services.AddScoped<IStorageClient, UpYunStorageClient>();
            services.AddHttpClient();
        }
    }
}
