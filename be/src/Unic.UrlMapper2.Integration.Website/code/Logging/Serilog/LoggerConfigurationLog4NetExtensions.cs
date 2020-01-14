namespace Unic.UrlMapper2.Integration.Website.Logging.Serilog
{
    using System;
    using global::Serilog;
    using global::Serilog.Configuration;
    using global::Serilog.Events;

    /// <summary>
    ///     Adds the WriteTo.Log4Net() extension method to <see cref="LoggerConfiguration" />.
    /// </summary>
    public static class LoggerConfigurationLog4NetExtensions
    {
        /// <summary>
        ///     Adds a sink that writes log events as documents to log4net.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="defaultLoggerName">
        ///     If events are logged using Serilog's <see cref="ILogger.ForContext{T}" /> method,
        ///     the type name will be used as the logger name (in line with log4net's behaviour. If no context type is specified,
        ///     Serilog will use this value (which defaults to <code>"serilog"</code>) as the logger name.
        /// </param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="supplyContextMessage">
        ///     Whether to supply a marker context message to use during the log. See:
        ///     https://logging.apache.org/log4net/release/manual/contexts.html#stacks
        /// </param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration Log4Net(
            this LoggerSinkConfiguration loggerConfiguration,
            string defaultLoggerName = "serilog",
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            bool supplyContextMessage = false)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));
            if (defaultLoggerName == null) throw new ArgumentNullException(nameof(defaultLoggerName));

            return loggerConfiguration.Sink(new Log4NetSink(defaultLoggerName, formatProvider, supplyContextMessage), restrictedToMinimumLevel);
        }
    }
}