namespace Unic.UrlMapper2.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Abstractions;
    using Sitecore.Annotations;
    using Sitecore.Sites;

    [UsedImplicitly]
    public class SiteExecutionFilter : ISiteExecutionFilter
    {
        private readonly BaseLog logger;

        public SiteExecutionFilter(BaseLog logger)
        {
            this.logger = logger;
        }

        public bool IsSiteAllowed(SiteContext siteContext, IList<string> allowedSites, IList<string> restrictedSites)
        {
            if (allowedSites == null)
            {
                this.logger.Warn($"Value '{nameof(allowedSites)}' cannot be null", null, this);
                return true;
            }

            if (restrictedSites == null)
            {
                this.logger.Warn($"Value '{nameof(restrictedSites)}' cannot be null", null, this);
                return true;
            }

            if (siteContext == null)
            {
                return true;
            }

            var siteInheritanceList = GetSiteInheritanceList(siteContext).ToList();

            // Check whether execution should prevented because of the blacklist
            if (restrictedSites.Any(siteInheritanceList.Contains))
            {
                return false;
            }

            // Check whether execution should prevented because of the whitelist
            return !allowedSites.Any() || allowedSites.Any(siteInheritanceList.Contains);
        }

        private static IEnumerable<string> GetSiteInheritanceList(SiteContext siteContext)
        {
            yield return siteContext.Name;

            var tenant = siteContext.Properties["tenant"];
            if (string.IsNullOrWhiteSpace(tenant)) yield break;

            yield return tenant;
        }
    }
}