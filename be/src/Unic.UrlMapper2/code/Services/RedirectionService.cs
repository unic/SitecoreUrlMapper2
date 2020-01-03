namespace Unic.UrlMapper2.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sitecore.Web;
    using Unic.UrlMapper2.Models;

    public class RedirectionService : IRedirectionService
    {
        private readonly IRedirectSearcher redirectSearcher;
        private readonly ISanitizer sanitizer;

        public RedirectionService(IRedirectSearcher redirectSearcher, ISanitizer sanitizer)
        {
            this.redirectSearcher = redirectSearcher;
            this.sanitizer = sanitizer;
        }

        public virtual void PerformRedirect(RedirectSearchData redirectSearchData, HttpContextBase httpContext)
        {
            if (redirectSearchData == null) return;
            this.sanitizer.SanitizeRedirectSearchData(redirectSearchData);

            var redirects = this.redirectSearcher.SearchRedirects(redirectSearchData)?.ToList();
            if (redirects == null || !redirects.Any()) return; // TODO: Add logging
            
            var redirect = this.FilterRedirects(redirects, redirectSearchData);
            if (redirect == null) return;

            this.PerformRedirect(redirect, httpContext);
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
            var wildcardMatches = enumerableRedirects.Where(r => r.WildcardEnabled && (redirectSearchData.SourceTerm?.StartsWith(r.Term) ?? false));
            var wildcardMatch = wildcardMatches.OrderByDescending(r => r.Term.Length).FirstOrDefault();

            return wildcardMatch;
        }

        protected virtual void PerformRedirect(Redirect redirect, HttpContextBase httpContext)
        {
            if (redirect == null || string.IsNullOrWhiteSpace(redirect.TargetUrl))
            {
                // TODO: Add logging about null arg
                return;
            }

            // TODO: Add logging about performing temporary and permanent redirect

            switch (redirect.RedirectType)
            {
                case RedirectType.Temporary:
                    httpContext.Response.Redirect(redirect.TargetUrl, true);
                    break;
                case RedirectType.Permanent:
                    httpContext.Response.RedirectPermanent(redirect.TargetUrl, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}