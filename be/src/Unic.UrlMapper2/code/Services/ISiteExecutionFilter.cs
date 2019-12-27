using System.Collections.Generic;

namespace Unic.UrlMapper2.Services
{
    using Sitecore.Sites;

    public interface ISiteExecutionFilter
    {
        bool IsSiteAllowed(SiteContext siteContext, IList<string> allowedSites, IList<string> restrictedSites);
    }
}
