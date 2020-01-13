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

        public RedirectSearchDataService(IUrlMapperContext context, BaseSettings settings)
        {
            this.context = context;
            this.settings = settings;
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

            // ... otherwise use the protocol provided from the HttpContext
            return args.HttpContext.Request.Url?.Scheme;
        }

        protected virtual string GetSourceProtocolForJssRedirectSearchData(HttpContextBase httpContext)
        {
            // Check if a request header should be used to determine the requested protocol
            var headerName = this.GetToggleableSettings(
                Constants.Settings.UseProtocolHeaderForJssProcessor,
                Constants.Settings.ProtocolHeaderForJssProcessor);

            if (string.IsNullOrWhiteSpace(headerName))
            {
                // TODO: Add logging about not defined header name
            }
            else
            {
                var value = httpContext.Request.Headers[headerName];
                if (!string.IsNullOrWhiteSpace(value)) return value;
            }

            // TODO: Add logging that header could not be found and fallback will be used

            // ... otherwise use the protocol provided from the HttpContext
            return httpContext.Request.Url?.Scheme;
        }

        protected virtual string GetToggleableSettings(string toggleSettingName, string valueSettingName) =>
            this.settings.GetBoolSetting(toggleSettingName, false)
                ? this.settings.GetSetting(valueSettingName)
                : null;

        protected virtual string GetSourceTermForJssRedirectSearchData(HttpContextBase httpContext)
        {
            if (httpContext == null) return null;

            var itemPath = httpContext.Request.QueryString["item"];
            return string.IsNullOrWhiteSpace(itemPath)
                ? null
                : this.StripVirtualFolderPath(itemPath);
        }

        protected virtual string GetEmbeddedLanguageForJssRedirectSearchData(HttpContextBase httpContext)
        {
            var headerName = this.settings.GetSetting(Constants.Settings.OriginalUrlHeaderForJssProcessor);
            if (string.IsNullOrWhiteSpace(headerName))
            {
                // TODO: Add logging
                return null;
            }

            var originalUrl = httpContext.Request.Headers[headerName];
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                // TODO: Add logging
                return null;
            }

            if (this.TryExtractLanguage(originalUrl, out var embeddedLanguage))
            {
                return embeddedLanguage?.Name;
            }
            else
            {
                // TODO: Add logging
                return null;
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