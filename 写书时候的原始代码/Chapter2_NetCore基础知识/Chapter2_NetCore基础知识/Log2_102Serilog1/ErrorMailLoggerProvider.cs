using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace YouZack.ErrorMail
{
    public class ErrorMailLoggerProvider : ILoggerProvider
    {
        private readonly IOptionsSnapshot<ErrorMailLoggerOptions> options;
        public ErrorMailLoggerProvider(IOptionsSnapshot<ErrorMailLoggerOptions> options)
        {
            this.options = options;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new ErrorMailLogger(categoryName, options);
        }

        public void Dispose()
        {
        }
    }
}
