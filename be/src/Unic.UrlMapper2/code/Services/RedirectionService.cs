namespace Unic.UrlMapper2.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore;
    using Sitecore.Web;
    using Unic.UrlMapper2.Models;

    public class RedirectionService : IRedirectionService
    {
        private readonly IRedirectSearcher redirectSearcher;

        public RedirectionService(IRedirectSearcher redirectSearcher)
        {
            this.redirectSearcher = redirectSearcher;
        }

        public virtual void PerformRedirect(RedirectSearchData redirectSearchData)
        {
            if (redirectSearchData == null) return;
            this.SanitizeRedirectSearchData(redirectSearchData);

            var redirects = this.redirectSearcher.SearchRedirects(redirectSearchData)?.ToList();
            if (redirects == null || !redirects.Any()) return; // TODO: Add logging
            
            var redirect = this.FilterRedirects(redirects, redirectSearchData);
            if (redirect == null) return;

            this.PerformRedirect(redirect);
        }

        public virtual string SanitizeTerm(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            value = value.Trim().ToLower();

            value = StringUtil.RemovePrefix('/', value);
            value = StringUtil.RemovePostfix('/', value);

            return value;
        }

        public virtual string SanitizeSiteName(string value) => value?.Trim().ToLower();

        protected virtual void SanitizeRedirectSearchData(RedirectSearchData redirectSearchData)
        {
            redirectSearchData.SourceTerm = this.SanitizeTerm(redirectSearchData.SourceTerm);
            redirectSearchData.SiteName = this.SanitizeSiteName(redirectSearchData.SiteName);

            redirectSearchData.EmbeddedLanguage = redirectSearchData.EmbeddedLanguage?.Trim().ToLower();
            redirectSearchData.Language = redirectSearchData.Language?.Trim().ToLower();
        }

        protected virtual Redirect FilterRedirects(IEnumerable<Redirect> redirects, RedirectSearchData redirectSearchData)
        {
            var enumerableRedirects = redirects.ToList();

            // If there is a strong redirect (one that doesn't have wildcards enabled), it has priority
            var strongRedirect = this.GetStrongRedirect(enumerableRedirects);
            if (strongRedirect != null) return strongRedirect;

            // ... otherwise we are going to check if there is any wildcard match available
            var wildcardMatch = this.GetWildcardMatch(redirectSearchData, enumerableRedirects);
            return wildcardMatch;
        }

        protected virtual Redirect GetStrongRedirect(IEnumerable<Redirect> enumerableRedirects) => enumerableRedirects.FirstOrDefault(r => !r.WildcardEnabled);

        protected virtual Redirect GetWildcardMatch(RedirectSearchData redirectSearchData, IEnumerable<Redirect> enumerableRedirects)
        {
            // We are going to take the one wildcard redirect which has the longest match within the term
            var wildcardMatches = enumerableRedirects.Where(r => r.WildcardEnabled && redirectSearchData.SourceTerm.StartsWith(r.Term));
            var wildcardMatch = wildcardMatches.OrderByDescending(r => r.Term.Length).FirstOrDefault();

            return wildcardMatch;
        }

        protected virtual void PerformRedirect(Redirect redirect)
        {
            // TODO: Add logging about performing temporary and permanent redirect

            WebUtil.Redirect(redirect.TargetUrl);
        }
    }
}