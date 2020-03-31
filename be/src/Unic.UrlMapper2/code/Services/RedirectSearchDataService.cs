namespace Unic.UrlMapper2.Services
{
    using System;
    using System.Web;
    using Sitecore;
    using Sitecore.Abstractions;
    using Sitecore.Pipelines.HttpRequest;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Models;
    using Constants = Definitions.Constants;

    public class RedirectSearchDataService : IRedirectSearchDataService
    {
        private readonly IUrlMapperContext context;
        private readonly BaseSettings settings;
        private readonly BaseLog logger;

        public RedirectSearchDataService(
            IUrlMapperContext context,
            BaseSettings settings,
            BaseLog logger)
        {
            this.context = context;
            this.settings = settings;
            this.logger = logger;
        }

        public virtual RedirectSearchData GetDefaultRedirectSearchData(HttpRequestArgs args) =>
            new RedirectSearchData(
                sourceTerm: this.GetSourceTermForDefaultRedirectSearchData(args),
                language: this.context.Language?.Name,
                siteName: this.context.Site?.Name?.ToLower(),
                sourceProtocol: this.GetSourceProtocolForDefaultRedirectSearchData(args));

        protected virtual string GetSourceTermForDefaultRedirectSearchData(HttpRequestArgs args)
        {
            var pathAndQuery = args?.RequestUrl?.PathAndQuery;
            if (string.IsNullOrWhiteSpace(pathAndQuery))
            {
                return null;
            }

            var startIndex = pathAndQuery.LastIndexOf(args.LocalPath, StringComparison.InvariantCultureIgnoreCase);
            return startIndex < 0 ? null : args.RequestUrl.PathAndQuery.Substring(startIndex);
        }

        public virtual RedirectSearchData GetJssRedirectSearchData(HttpContextBase httpContext) =>
            new RedirectSearchData(
                sourceTerm: this.GetSourceTermForJssRedirectSearchData(httpContext),
                language: this.context.Language?.Name,
                siteName: this.context.Site?.Name,
                sourceProtocol: this.GetSourceProtocolForJssRedirectSearchData(httpContext));

        protected virtual string GetSourceProtocolForDefaultRedirectSearchData(HttpRequestArgs args)
        {
            // Check if a request header should be used to determine the requested protocol
            var headerName = this.GetToggleableSettings(
                Constants.Settings.UseProtocolHeaderForDefaultProcessor,
                Constants.Settings.ProtocolHeaderForDefaultProcessor);

            // ReSharper disable once InvertIf
            if (!string.IsNullOrWhiteSpace(headerName))
            {
                var value = args.HttpContext.Request.Headers[headerName];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }

                this.logger.Warn($"Header {headerName} could not be found in the current request. Falling back to current request scheme", this);
            }

            // ... otherwise use the protocol provided from the HttpContext
            return args.HttpContext.Request.Url?.Scheme;
        }

        protected virtual string GetSourceProtocolForJssRedirectSearchData(HttpContextBase httpContext)
        {
            // Check if a request header should be used to determine the requested protocol
            var headerName = this.GetToggleableSettings(
                Constants.Settings.UseProtocolHeaderForJssProcessor,
                Constants.Settings.ProtocolHeaderForJssProcessor);

            if (!string.IsNullOrWhiteSpace(headerName))
            {
                var value = httpContext.Request.Headers[headerName];
                if (!string.IsNullOrWhiteSpace(value)) return value;
            }

            this.logger.Warn($"Header {headerName} could not be found in the current JSS request. Falling back to current request scheme", this);

            // ... otherwise use the protocol provided from the HttpContext
            return httpContext.Request.Url?.Scheme;
        }

        protected virtual string GetToggleableSettings(string toggleSettingName, string valueSettingName) =>
            this.settings.GetBoolSetting(toggleSettingName, false)
                ? this.settings.GetSetting(valueSettingName)
                : default;

        protected virtual string GetSourceTermForJssRedirectSearchData(HttpContextBase httpContext)
        {
            if (httpContext is null) return default;

            var itemPath = httpContext.Request.QueryString["item"];

            // https://github.com/Sitecore/jss/issues/140 describes an issue where query string parameters are proxied from the headless proxy
            // to the LayoutService in the "item" parameter, causing 404s. If you have applied the suggested fix in your headless proxy, you will need to pass
            // the originally requested relative url including query strings to Sitecore, as otherwise query strings would be lost and not considered
            // by UrlMapper.
            var headerName = this.GetToggleableSettings(
                Constants.Settings.UseOriginalUrlHeaderForJssProcessor,
                Constants.Settings.OriginalUrlHeaderForJssProcessor);

            // ReSharper disable once InvertIf
            if (!string.IsNullOrWhiteSpace(headerName))
            {
                // Prepare proper relative path including query strings for further processing if original URL has been passed in a header
                var originalUrl = httpContext.Request.Headers[headerName];
                if (!string.IsNullOrWhiteSpace(originalUrl))
                {
                    itemPath = originalUrl.Substring(originalUrl.IndexOf(itemPath, StringComparison.Ordinal));
                }
            }

            return string.IsNullOrWhiteSpace(itemPath)
                ? default
                : this.StripVirtualFolderPath(itemPath);
        }

        protected virtual string StripVirtualFolderPath(string path)
        {
            // this has been copied from the out of the box JSS context item resolver as it is not directly accessible
            var virtualFolder = this.context.Site.VirtualFolder;

            // ReSharper disable once InvertIf
            if (!string.IsNullOrWhiteSpace(path) && virtualFolder.Length > 0 && virtualFolder != "/")
            {
                var str = StringUtil.EnsurePostfix('/', virtualFolder);
                if (StringUtil.EnsurePostfix('/', path).StartsWith(str, StringComparison.InvariantCultureIgnoreCase))
                {
                    path = StringUtil.Mid(path, str.Length);
                }
            }
            return path;
        }
    }
}