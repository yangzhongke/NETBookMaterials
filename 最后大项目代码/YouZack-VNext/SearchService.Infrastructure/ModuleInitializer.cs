using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using SearchService.Domain;
using SearchService.WebAPI.Options;
using Zack.Commons;

namespace SearchService.Infrastructure
{
    internal class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IElasticClient>(sp =>
            {
                var option = sp.GetRequiredService<IOptions<ElasticSearchOptions>>();
                var settings = new ConnectionSettings(option.Value.Url);
                return new ElasticClient(settings);
            });
            services.AddScoped<ISearchRepository, SearchRepository>();
        }
    }
}
