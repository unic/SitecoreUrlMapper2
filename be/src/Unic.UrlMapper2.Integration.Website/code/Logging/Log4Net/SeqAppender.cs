namespace Unic.UrlMapper2.Integration.Website.Logging.Log4Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using log4net.Appender;
    using log4net.spi;
    using Newtonsoft.Json;
    using Sitecore;

    [Sitecore.Annotations.UsedImplicitly]
    public class SeqAppender : BufferingAppenderSkeleton
    {
        private const string BulkUploadResource = "api/events/raw";

        private const string ApiKeyHeaderName = "X-Seq-ApiKey";

        private static readonly IDictionary<string, string> LevelMap = new Dictionary<string, string>
        {
            { "DEBUG", "Debug" },
            { "INFO", "Information" },
            { "WARN", "Warning" },
            { "ERROR", "Error" },
            { "FATAL", "Fatal" }
        };

        private readonly HttpClient httpClient = new HttpClient();

        public string ServerUrl
        {
            get => this.httpClient.BaseAddress?.OriginalString;
            set => this.httpClient.BaseAddress = new Uri(StringUtil.EnsurePostfix('/', value));
        }

        public string ApiKey { get; set; }

        public string Timeout
        {
            get => this.httpClient.Timeout.ToString();
            set => this.httpClient.Timeout = TimeSpan.Parse(value);
        }

        public string Application { get; set; }

        public string Environment { get; set; }

        public string Category { get; set; }

        public override void OnClose()
        {
            base.OnClose();

            this.httpClient.Dispose();
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (this.ServerUrl == null)
            {
                return;
            }

            var payload = this.GetPayload(events);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            if (!string.IsNullOrWhiteSpace(this.ApiKey))
            {
                content.Headers.Add(ApiKeyHeaderName, this.ApiKey);
            }

            using (var result = this.httpClient.PostAsync(BulkUploadResource, content).Result)
            {
                if (!result.IsSuccessStatusCode)
                {
                    this.ErrorHandler.Error($"Received failed result {result.StatusCode}: {result.Content.ReadAsStringAsync().Result}");
                }
            }
        }

        protected virtual string GetPayload(LoggingEvent[] events)
        {
            var rawEventsList = new RawEvents
            {
                Events = events.Select(this.ToEvent).ToArray()
            };

            return JsonConvert.SerializeObject(rawEventsList);
        }

        protected virtual RawEvent ToEvent(LoggingEvent loggingEvent) => new RawEvent
        {
            Level = this.MapLevel(loggingEvent.Level),
            MessageTemplate = GetMessageTemplate(loggingEvent),
            Timestamp = new DateTimeOffset(loggingEvent.TimeStamp, DateTimeOffset.Now.Offset),
            Properties = this.GetProperties(loggingEvent),
            Exception = loggingEvent.GetExceptionStrRep()
        };

        protected virtual string MapLevel(Level level) => LevelMap.TryGetValue(level.Name, out var mappedLevel) ? mappedLevel : "Information";

        protected Dictionary<string, object> GetProperties(LoggingEvent loggingEvent)
        {
            var properties = new Dictionary<string, object>
            {
                { nameof(this.Application), this.Application },
                { nameof(this.Environment), this.Environment },
                { nameof(this.Category), this.Category ?? loggingEvent.LoggerName },
                { nameof(loggingEvent.LoggerName), loggingEvent.LoggerName },
                { nameof(loggingEvent.ThreadName), loggingEvent.ThreadName },
                { nameof(loggingEvent.UserName), loggingEvent.UserName },
                { nameof(loggingEvent.Identity), loggingEvent.Identity },
                { nameof(loggingEvent.Domain), loggingEvent.Domain }
            };

            return properties;
        }

        private static string GetMessageTemplate(LoggingEvent loggingEvent) => loggingEvent.RenderedMessage;
    }
}