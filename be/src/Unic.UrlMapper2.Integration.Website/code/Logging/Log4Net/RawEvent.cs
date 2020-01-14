namespace Unic.UrlMapper2.Integration.Website.Logging.Log4Net
{
    using System;
    using System.Collections.Generic;

    public class RawEvent
    {
        public DateTimeOffset Timestamp { get; set; }

        public string Level { get; set; }

        public string MessageTemplate { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public string Exception { get; set; }
    }
}