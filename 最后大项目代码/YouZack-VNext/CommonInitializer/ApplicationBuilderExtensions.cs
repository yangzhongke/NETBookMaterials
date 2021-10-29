using Zack.EventBus;

namespace CommonInitializer
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseZackDefault(this IApplicationBuilder app)
        {
            app.UseEventBus();
            app.UseCors();//启用Cors
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
