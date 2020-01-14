namespace Unic.UrlMapper2.Services
{
    using System;
    using System.Web;
    using Sitecore;
    using Sitecore.Abstractions;
    using Sitecore.Globalization;
    using Sitecore.Pipelines.HttpRequest;
    using Sitecore.Web;
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
                sourceTerm: args.LocalPath,
                language: this.context.Language?.Name,
                siteName: this.context.Site?.Name?.ToLower(),
                embeddedLanguage: this.context.FilePathLanguage?.Name,
                sourceProtocol: this.GetSourceProtocolForDefaultRedirectSearchData(args));

        public virtual RedirectSearchData GetJssRedirectSearchData(HttpContextBase httpContext) =>
            new RedirectSearchData(
                sourceTerm: this.GetSourceTermForJssRedirectSearchData(httpContext),
                language: this.context.Language?.Name,
                siteName: this.context.Site?.Name,
                embeddedLanguage: this.GetEmbeddedLanguageForJssRedirectSearchData(httpContext),
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
            return string.IsNullOrWhiteSpace(itemPath)
                ? default
                : this.StripVirtualFolderPath(itemPath);
        }

        protected virtual string GetEmbeddedLanguageForJssRedirectSearchData(HttpContextBase httpContext)
        {
            var headerName = this.settings.GetSetting(Constants.Settings.OriginalUrlHeaderForJssProcessor);
            if (string.IsNullOrWhiteSpace(headerName))
            {
                this.logger.Warn($"Header {headerName} could not be found in the current JSS request.", this);
                return default;
            }

            var originalUrl = httpContext.Request.Headers[headerName];
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                this.logger.Warn($"Header {headerName} does not contain a value.", this);
                return default;
            }

            if (this.TryExtractLanguage(originalUrl, out var embeddedLanguage))
            {
                return embeddedLanguage?.Name;
            }
            else
            {
                this.logger.Warn($"Failed to extract language for current request", this);
                return default;
            }
        }

        protected virtual bool TryExtractLanguage(string url, out Language language)
        {
            // check Sitecore.Pipelines.PreprocessRequest.StripLanguage
            return Language.TryParse(this.ExtractLanguageName(url), out language);
        }

        protected virtual string ExtractLanguageName(string filePath)
        {
            var languageName = WebUtil.ExtractLanguageName(filePath);
            return !string.IsNullOrEmpty(languageName) ? languageName : null;
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