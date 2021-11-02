using Microsoft.AspNetCore.Builder;
using Zack.EventBus;

namespace CommonInitializer
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseZackDefault(this IApplicationBuilder app)
        {
            app.UseEventBus();
            app.UseCors();//启用Cors
            app.UseForwardedHeaders();
            //app.UseHttpsRedirection();//不能与ForwardedHeaders很好的工作，而且webapi项目也没必要配置这个
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
