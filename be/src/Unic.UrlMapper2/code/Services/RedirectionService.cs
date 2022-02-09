namespace Unic.UrlMapper2.Services
{
    using Sitecore.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Sitecore.Data.Items;
    using Sitecore.Links;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Definitions;
    using Unic.UrlMapper2.Models;

    public class RedirectionService : IRedirectionService
    {
        private readonly IRedirectSearcher redirectSearcher;
        private readonly ISanitizer sanitizer;
        private readonly BaseLog logger;
        private readonly IUrlMapperContext context;
        private readonly BaseLinkManager linkManager;
        private readonly BaseMediaManager mediaManager;

        public RedirectionService(
            IRedirectSearcher redirectSearcher,
            ISanitizer sanitizer,
            BaseLog logger,
            IUrlMapperContext context,
            BaseLinkManager linkManager,
            BaseMediaManager mediaManager)
        {
            this.redirectSearcher = redirectSearcher;
            this.sanitizer = sanitizer;
            this.logger = logger;
            this.context = context;
            this.linkManager = linkManager;
            this.mediaManager = mediaManager;
        }

        public virtual void PerformRedirect(RedirectSearchData redirectSearchData, HttpContextBase httpContext)
        {
            if (redirectSearchData is null) return;
            this.sanitizer.SanitizeRedirectSearchData(redirectSearchData);

            var redirects = this.redirectSearcher.SearchRedirects(redirectSearchData)?.ToList();
            if (redirects is null || !redirects.Any())
            {
                this.logger.Debug($"No redirects found for term {redirectSearchData.SourceTerm}", this);
            }

            var redirect = this.FilterRedirects(redirects, redirectSearchData);
            if (redirect is null) return;

            var additionalTargetData = this.GetAdditionalTargetUrlData(redirectSearchData.SourceTermOriginal, redirect);

            this.PerformRedirect(redirect, httpContext, additionalTargetData);
        }

        protected virtual string GetAdditionalTargetUrlData(string sourceTerm, Redirect redirect)
        {
            if (!redirect.RegexEnabled) return default;

            var match = Regex.Match(sourceTerm, redirect.Term, RegexOptions.IgnoreCase);
            if (match.Groups.Count <= 1) return default;

            string additionalTargetData = null;

            // We most probably have capture groups defined within the regex. If so, we are going to apply them to the target url
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 1; i < match.Groups.Count; i++)
            {
                var groupValue = match.Groups[i].Value;
                if (!string.IsNullOrWhiteSpace(groupValue))
                {
                    additionalTargetData += groupValue;
                }
            }

            return additionalTargetData;
        }

        protected virtual Redirect FilterRedirects(IEnumerable<Redirect> redirects, RedirectSearchData redirectSearchData)
        {
            if (redirects is null) return default;

            var enumerableRedirects = redirects.ToList();

            // If there is a strong redirect (one that doesn't have regex enabled), it has priority
            var strongRedirect = this.GetStrongRedirect(enumerableRedirects);
            if (strongRedirect != null) return strongRedirect;

            // ... otherwise we are going to check if there is any regex match available
            var regexMatch = this.GetRegexMatch(redirectSearchData, enumerableRedirects);
            return regexMatch;
        }

        protected virtual Redirect GetStrongRedirect(IEnumerable<Redirect> enumerableRedirects) => enumerableRedirects.FirstOrDefault(r => !r.RegexEnabled);

        protected virtual Redirect GetRegexMatch(RedirectSearchData redirectSearchData, IEnumerable<Redirect> enumerableRedirects)
        {
            var sourceTerm = redirectSearchData?.SourceTerm;
            if (string.IsNullOrWhiteSpace(sourceTerm)) return default;

            // We are going to take the one regex redirect which has the longest match within the term
            var regexMatches = enumerableRedirects.Where(r => r.RegexEnabled && Regex.IsMatch(input: sourceTerm, pattern: r.Term));
            var regexMatch = regexMatches.OrderByDescending(r => r.Term.Length).FirstOrDefault();

            return regexMatch;
        }

        protected virtual void PerformRedirect(Redirect redirect, HttpContextBase httpContext, string additionalTargetData)
        {
            if (redirect?.ItemId is null)
            {
                this.logger.Debug("Incomplete redirect information provided. Redirect will be aborted.", this);
                return;
            }

            var redirectItem = this.GetRedirectItem(redirect);
            if (redirectItem is null)
            {
                this.logger.Error($"Failed to perform redirect. Item with ID {redirect.ItemId} could not be found", this);
                return;
            }

            var targetUrl = this.GetTargetUrl(redirectItem, additionalTargetData);
            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                this.logger.Error($"Failed to determine target url for redirect item {redirectItem.ID}", this);
                return;
            }

            this.logger.Debug($"Performing {redirect.RedirectType} redirect to {targetUrl}", this);

            switch (redirect.RedirectType)
            {
                case RedirectType.Temporary:
                    httpContext.Response.Redirect(targetUrl, true);
                    break;
                case RedirectType.Permanent:
                    httpContext.Response.RedirectPermanent(targetUrl, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual Item GetRedirectItem(Redirect redirect) => this.context.Database.GetItem(redirect.ItemId);

        protected virtual string GetTargetUrl(Item item, string additionalTargetData)
        {
            if (item == null) return default;

            // Based on https://doc.sitecore.com/developers/93/sitecore-experience-manager/en/how-to-access-general-link-fields.html
            Sitecore.Data.Fields.LinkField linkField = item.Fields[Constants.Fields.Redirect.Target];

            var targetUrl = string.Empty;
            switch (linkField.LinkType)
            {
                case "internal":
                    var internalTargetItem = linkField.TargetItem;
                    if (internalTargetItem is null) return default;

                    targetUrl = this.linkManager.GetItemUrl(internalTargetItem, this.GetUrlOptions());
                    break;
                case "external":
                case "mailto":
                case "anchor":
                case "javascript":
                    targetUrl = linkField.Url;
                    break;
                case "media":
                    var mediaTargetItem = linkField.TargetItem;
                    if (mediaTargetItem is null) return default;

                    var media = new MediaItem(mediaTargetItem);
                    targetUrl = Sitecore.StringUtil.EnsurePrefix('/', this.mediaManager.GetMediaUrl(media));
                    break;
                case "":
                    break;
                default:
                    this.logger.Error($"Unknown link type {linkField.LinkType} in {item.Paths.FullPath}", this);
                    break;
            }

            return $"{targetUrl}{additionalTargetData}";
        }

        protected virtual UrlOptions GetUrlOptions()
        {
            var options = this.linkManager.GetDefaultUrlOptions();
            options.SiteResolving = true;
            options.AlwaysIncludeServerUrl = false;

            return options;
        }
    }
}