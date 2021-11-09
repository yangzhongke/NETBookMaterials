using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YouZack.ErrorMail
{
    public class ErrorMailLogger : ILogger
    {
        private readonly string categoryName;
        private readonly IOptionsSnapshot<ErrorMailLoggerOptions> options;
        /// <summary>
        /// 用来记录发送过的消息，以实现一个时间段内发送过则不重复发送
        /// Key是消息，Value是上一次的发送时间
        /// </summary>
        private readonly ConcurrentDictionary<string,DateTime> sendedMessagesCache 
            = new ConcurrentDictionary<string, DateTime>();

        public ErrorMailLogger(string categoryName, IOptionsSnapshot<ErrorMailLoggerOptions> options)
        {
            this.categoryName = categoryName;
            this.options = options;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error || logLevel == LogLevel.Critical;
        }

        /// <summary>
        /// 清除sendedMessagesCache中的过期数据
        /// </summary>
        private void CleanExpiredMessagesCache()
        {
            int intervalSec = options.Value.IntervalSeconds;
            var expiredDateTime = DateTime.Now.AddSeconds(-intervalSec);
            List<string> keysToBeRemoved = new List<string>();
            foreach (var kv in this.sendedMessagesCache)
            {
                if (kv.Value < expiredDateTime)
                {
                    keysToBeRemoved.Add(kv.Key);
                }
            }
            foreach(var key in keysToBeRemoved)
            {
                this.sendedMessagesCache.TryRemove(key,out DateTime dt);               
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            CleanExpiredMessagesCache();
            var opt = options.Value;
            if (opt.SendSameErrorOnlyOnce&& sendedMessagesCache.ContainsKey(message))
            {
                return;
            }
            string body = FormatLayout(opt.Body, message, categoryName, logLevel, eventId, exception);
            string subject = FormatLayout(opt.Subject, message, categoryName, logLevel, eventId, exception);
            SendMail(subject, body);
            sendedMessagesCache[message] = DateTime.Now;
        }

        private void SendMail(string subject,string body)
        {
            var opt = options.Value;
            using (var mailMessage = new MailMessage())
            using (var client = new SmtpClient())
            {
                if(opt.To!=null&&opt.To.Length>0)
                {
                    mailMessage.To.Add(string.Join(',', opt.To));
                }                
                mailMessage.From = new MailAddress(opt.From);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = false;
                client.Host = opt.SmtpServer;
                client.Port = opt.SmtpPort;
                client.EnableSsl = opt.SmtpEnableSsl;
                client.Credentials = new NetworkCredential(opt.SmtpUserName, opt.SmtpPassword);
                client.Send(mailMessage);
            }
        }
        //进行占位符替换
        private static string FormatLayout(string template, string message,string categoryName,
            LogLevel logLevel, EventId eventId, Exception exception)
        {
            string result = template.Replace("${message}", message).Replace("${logLevel}", logLevel.ToString())
                .Replace("${categoryName}", categoryName).Replace("${newline}", Environment.NewLine);
            if (exception != null)
            {
                result = result.Replace("${exception}", exception.StackTrace);
            }
            result = result.Replace("${datetime}", DateTime.Now.ToString())
                .Replace("${machinename}", Environment.MachineName)
                .Replace("${eventId}", eventId.ToString());
            return result;
        }

        private class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new NullDisposable();

            public void Dispose()
            {
            }
        }
    }
}