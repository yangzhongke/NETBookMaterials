using System;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogInterpolatedCritical(this ILogger logger, FormattableString formattableString,
            Exception? exception = default, EventId eventId = default)
        {
            //todo: 插值字符串支持$"a={a,3:C}"这样的写法，目前这样不支持，需要解析，参考https://gist.github.com/artemious7/c7d9856e128a8b2e9e92d096ca0e69ee#file-serilog-loggerstringinterpolationextensions-cs
            logger.LogCritical(eventId, exception, formattableString.Format, formattableString.GetArguments());
        }

        public static void LogInterpolatedDebug(this ILogger logger, FormattableString formattableString,
            Exception? exception = default, EventId eventId = default)
        {
            logger.LogDebug(eventId, exception, formattableString.Format, formattableString.GetArguments());
        }

        public static void LogInterpolatedError(this ILogger logger, FormattableString formattableString,
            Exception? exception = default, EventId eventId = default)
        {
            logger.LogError(eventId, exception, formattableString.Format, formattableString.GetArguments());
        }

        public static void LogInterpolatedInformation(this ILogger logger, FormattableString formattableString,
            Exception? exception = default, EventId eventId = default)
        {
            logger.LogInformation(eventId, exception, formattableString.Format, formattableString.GetArguments());
        }

        public static void LogInterpolatedTrace(this ILogger logger, FormattableString formattableString,
            Exception? exception = default, EventId eventId = default)
        {
            logger.LogTrace(eventId, exception, formattableString.Format, formattableString.GetArguments());
        }

        public static void LogInterpolatedWarning(this ILogger logger, FormattableString formattableString,
            Exception? exception = default, EventId eventId = default)
        {
            logger.LogWarning(eventId, exception, formattableString.Format, formattableString.GetArguments());
        }
    }
}
