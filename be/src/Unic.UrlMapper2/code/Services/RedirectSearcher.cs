namespace Unic.UrlMapper2.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using MoreLinq;
    using Sitecore.Abstractions;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Unic.UrlMapper2.ContentSearch.SearchResults;
    using Unic.UrlMapper2.Definitions;
    using Unic.UrlMapper2.Models;

    public class RedirectSearcher : IRedirectSearcher
    {
        private readonly BaseSettings settings;
        private readonly ITemplateService templateService;
        private readonly BaseLog logger;

        public RedirectSearcher(
            BaseSettings settings,
            ITemplateService templateService,
            BaseLog logger)
        {
            this.settings = settings;
            this.templateService = templateService;
            this.logger = logger;
        }

        public IEnumerable<Redirect> SearchRedirects(RedirectSearchData redirectSearchData)
        {
            if (redirectSearchData == null) return null;

            var indexName = this.GetIndexName();
            if (string.IsNullOrWhiteSpace(indexName)) return null;

            IEnumerable<RedirectSearchResultItem> results;
            using (var searchContext = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
            {
                var queryable = this.GetSearchQuery(searchContext, redirectSearchData);
                results = queryable.ToList().DistinctBy(r => r.ItemId)?.ToList();
            }

            if (results != null && results.Any())
            {
                return results.Select(r => this.MapToSearchResult(r, redirectSearchData));
            }

            this.logger.Debug($"No results found for current search data (term: {redirectSearchData.SourceTerm})", this);
            return null;
        }

        protected virtual Redirect MapToSearchResult(RedirectSearchResultItem redirectSearchResultItem, RedirectSearchData redirectSearchData)
        {
            if (!Enum.TryParse<RedirectType>(redirectSearchResultItem.RedirectType, ignoreCase: true, out var redirectType))
            {
                this.logger.Error($"Failed to parse redirect type {redirectSearchResultItem.RedirectType}", this);
            }

            if (!Enum.TryParse<SourceProtocol>(redirectSearchResultItem.SourceProtocol, ignoreCase: true, out var sourceProtocol))
            {
                this.logger.Error($"Failed to parse source protocol {redirectSearchResultItem.SourceProtocol}", this);
            }

            return new Redirect
            {
                RedirectSearchData = redirectSearchData,
                RedirectType = redirectType,
                TargetUrl = redirectSearchResultItem.TargetUrl,
                SourceProtocol = sourceProtocol,
                IncludeEmbeddedLanguage = redirectSearchResultItem.AllowEmbeddedLanguage,
                WildcardEnabled = redirectSearchResultItem.WildcardEnabled,
                Term = redirectSearchResultItem.SourceTerm
            };
        }

        protected virtual IQueryable<RedirectSearchResultItem> GetSearchQuery(IProviderSearchContext searchContext, RedirectSearchData redirectSearchData)
        {
            var queryable = searchContext.GetQueryable<RedirectSearchResultItem>()
                .Filter(this.GetVersionPredicate(redirectSearchData))
                .Filter(this.GetTemplatePredicate(redirectSearchData))
                .Filter(this.GetSitePredicate(redirectSearchData))
                .Filter(this.GetProtocolPredicate(redirectSearchData))
                .Filter(this.GetLanguagePredicate(redirectSearchData))
                .Filter(this.GetTermPredicate(redirectSearchData));

            return queryable;
        }

        protected virtual Expression<Func<RedirectSearchResultItem, bool>> GetVersionPredicate(RedirectSearchData redirectSearchData) =>
            r => r.IsLatestVersion;

        protected virtual Expression<Func<RedirectSearchResultItem, bool>> GetTemplatePredicate(RedirectSearchData redirectSearchData) =>
            r => r.TemplateId == this.templateService.GetSharedRedirectTemplateId() || r.TemplateId == this.templateService.GetRedirectTemplateId();

        protected virtual Expression<Func<RedirectSearchResultItem, bool>> GetProtocolPredicate(RedirectSearchData redirectSearchData) =>
            r => r.SourceProtocol == redirectSearchData.SourceProtocol || r.SourceProtocol == Constants.Markers.AnyProtocolMarker;

        protected virtual Expression<Func<RedirectSearchResultItem, bool>> GetSitePredicate(RedirectSearchData redirectSearchData) =>
            r => r.SiteName == redirectSearchData.SiteName || r.SiteName == Constants.Markers.GlobalSiteMarker;

        protected virtual Expression<Func<RedirectSearchResultItem, bool>> GetLanguagePredicate(RedirectSearchData redirectSearchData) =>
            r => redirectSearchData.ContainsEmbeddedLanguage && r.AllowEmbeddedLanguage || !redirectSearchData.ContainsEmbeddedLanguage;

        protected virtual Expression<Func<RedirectSearchResultItem, bool>> GetTermPredicate(RedirectSearchData redirectSearchData)
        {
            // It would be great to use a predicate like the following for the wildcard matches:
            // r.WildcardEnabled && redirectSearchData.SourceTerm.StartsWith(r.SourceTerm)
            // However, this seems to generate some really weird SolR queries, resulting in no matches.
            // Therefore we have to load all entries being a wildcard redirect and then do the filtering in memory.
            return r => !r.WildcardEnabled && r.SourceTerm == redirectSearchData.SourceTerm || r.WildcardEnabled;
        }

        protected virtual string GetIndexName() => this.settings.GetSetting(Constants.Settings.ActiveIndex);
    }
}