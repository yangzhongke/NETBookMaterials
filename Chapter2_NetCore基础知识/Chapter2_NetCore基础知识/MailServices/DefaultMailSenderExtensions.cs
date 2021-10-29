using MailServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DefaultMailSenderExtensions
    {
        public static IServiceCollection AddDefaultMailSender(this IServiceCollection services)
        {
            return services.AddSingleton<IMailSender, DefaultMailSender>();
        }
    }
}