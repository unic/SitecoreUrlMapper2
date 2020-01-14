namespace Unic.UrlMapper2.Integration.Website.Logging.Log4Net
{
    using System.Collections.Generic;

    public class StructuredLogInfo
    {
        public string Context { get; set; }

        public string Message { get; set; }

        public string MessageTemplate { get; set; }

        public string Controller { get; set; }

        public string RequestId { get; set; }

        public object Arguments { get; set; }

        public IEnumerable<(string name, object value)> GetArguments()
        {
            var type = this.Arguments?.GetType();
            var properties = type?.GetProperties();

            if (properties == null) yield break;

            foreach (var propertyInfo in properties)
            {
                yield return (name: propertyInfo.Name, value: propertyInfo.GetValue(this.Arguments));
            }
        }

        public override string ToString() => string.Join(";", this.GetValues());

        protected virtual IEnumerable<string> GetValues()
        {
            yield return this.Message ?? "unknown";
            yield return this.Context ?? "unknown";
            yield return this.RequestId ?? "unknown";
            yield return this.Controller ?? "unknown";

            foreach (var (name, value) in this.GetArguments())
            {
                yield return value?.ToString() ?? "unknown";
            }
        }
    }
}