using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using YouZack.ErrorMail;

namespace Microsoft.Extensions.Logging
{
public static class ErrorMailLoggerExtensions
{
    public static ILoggingBuilder AddErrorMail(this ILoggingBuilder builder,
        Action<ErrorMailLoggerOptions> configure)
    {
        var sd = ServiceDescriptor.Singleton<ILoggerProvider, ErrorMailLoggerProvider>();
        builder.Services.TryAddEnumerable(sd);
        builder.Services.Configure(configure);
        return builder;
    }
}
}
