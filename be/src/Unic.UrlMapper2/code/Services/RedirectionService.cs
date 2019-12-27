namespace Unic.UrlMapper2.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            var redirects = this.redirectSearcher.SearchRedirects(redirectSearchData)?.ToList();
            if (redirects == null || !redirects.Any()) return; // TODO: Add logging
            
            var redirect = this.FilterRedirects(redirects);
            if (redirect == null) return;

            this.PerformRedirect(redirect);
        }

        protected virtual Redirect FilterRedirects(IEnumerable<Redirect> redirects)
        {
            return redirects?.FirstOrDefault();

            // TODO: Add logging about multiple redirects
        }

        protected virtual void PerformRedirect(Redirect redirect)
        {
            throw new NotImplementedException();
        }
    }
}