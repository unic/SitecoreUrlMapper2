namespace Unic.UrlMapper2.Integration.Website.Logging.Serilog
{
    using System;
    using global::Serilog.Core;
    using global::Serilog.Debugging;
    using global::Serilog.Events;
    using log4net;

    public class Log4NetSink : ILogEventSink
    {
        private const string ContextMessage = "Serilog-Log4NetSink";
        private readonly string defaultLoggerName;
        private readonly IFormatProvider formatProvider;
        private readonly bool supplyContextMessage;

        public Log4NetSink(string defaultLoggerName, IFormatProvider formatProvider = null, bool supplyContextMessage = false)
        {
            this.defaultLoggerName = defaultLoggerName ?? throw new ArgumentNullException(nameof(defaultLoggerName));
            this.formatProvider = formatProvider;
            this.supplyContextMessage = supplyContextMessage;
        }

        public void Emit(LogEvent logEvent)
        {
            var loggerName = this.defaultLoggerName;

            if (logEvent.Properties.TryGetValue(Constants.SourceContextPropertyName, out var sourceContext))
            {
                if (sourceContext is ScalarValue sv && sv.Value is string name)
                {
                    loggerName = name;
                }
            }

            var message = logEvent.RenderMessage(this.formatProvider);
            var exception = logEvent.Exception;

            var logger = LogManager.GetLogger(loggerName);

            using (this.supplyContextMessage ? NDC.Push(ContextMessage) : new NullDisposable())
            {
                switch (logEvent.Level)
                {
                    case LogEventLevel.Verbose:
                    case LogEventLevel.Debug:
                        logger.Debug(message, exception);
                        break;
                    case LogEventLevel.Information:
                        logger.Info(message, exception);
                        break;
                    case LogEventLevel.Warning:
                        logger.Warn(message, exception);
                        break;
                    case LogEventLevel.Error:
                        logger.Error(message, exception);
                        break;
                    case LogEventLevel.Fatal:
                        logger.Fatal(message, exception);
                        break;
                    default:
                        SelfLog.WriteLine("Unexpected logging level, writing to log4net as Info");
                        logger.Info(message, exception);
                        break;
                }
            }
        }

        private class NullDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}