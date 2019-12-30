namespace Unic.UrlMapper2.Services
{
    using Sitecore.Abstractions;
    using Sitecore.Pipelines.HttpRequest;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Definitions;
    using Unic.UrlMapper2.Models;

    public class RedirectSearchDataService : IRedirectSearchDataService
    {
        private readonly IContext context;
        private readonly BaseSettings settings;

        public RedirectSearchDataService(IContext context, BaseSettings settings)
        {
            this.context = context;
            this.settings = settings;
        }

        public virtual RedirectSearchData GetDefaultRedirectSearchData(HttpRequestArgs args)
        {
            return new RedirectSearchData(
                sourceTerm: args.LocalPath,
                language: this.context.Language?.Name,
                siteName: this.context.Site?.Name?.ToLower(),
                embeddedLanguage: this.context.FilePathLanguage?.Name,
                sourceProtocol: this.GetSourceProtocolForDefaultRedirectSearchData(args));
        }

        protected virtual string GetSourceProtocolForDefaultRedirectSearchData(HttpRequestArgs args)
        {
            // Check if a request header should be used to determine the requested protocol
            // ReSharper disable once InvertIf
            if (this.settings.GetBoolSetting(Constants.Settings.UseProtocolHeaderForDefaultProcessor, false))
            {
                var headerName = this.settings.GetSetting(Constants.Settings.ProtocolHeaderForDefaultProcessor);
                if (string.IsNullOrWhiteSpace(headerName))
                {
                    // TODO: Add logging about not defined header name
                }
                else
                {
                    var value = args.HttpContext.Request.Headers[headerName];
                    if (!string.IsNullOrWhiteSpace(value)) return value;
                }

                // TODO: Add logging that header could not be found and fallback will be used
            }

            // ... otherwise use the protocol provided from the HttpContext
            return args.HttpContext?.Request.Url?.Scheme;
        }
    }
}