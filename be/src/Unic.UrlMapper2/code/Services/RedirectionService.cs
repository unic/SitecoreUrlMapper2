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
            
            var redirect = this.FilterRedirects(redirects);
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

        protected virtual Redirect FilterRedirects(IEnumerable<Redirect> redirects)
        {
            return redirects?.FirstOrDefault();

            // TODO: Add logging about multiple redirects
        }

        protected virtual void PerformRedirect(Redirect redirect)
        {
            // TODO: Add logging about performing redirect

            WebUtil.Redirect(redirect.TargetUrl);
        }
    }
}