namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore;
    using Sitecore.ContentSearch;
    using Sitecore.Data.Items;
    using Sitecore.Sites;
    using Sitecore.Web;
    using Unic.UrlMapper2.Services;

    [Sitecore.Annotations.UsedImplicitly]
    public class SiteNameComputedField : UrlMapperComputedFieldBase
    {
        protected override object Compute(IIndexable indexable)
        {
            var siteInfo = this.GetSiteInfo(indexable as SitecoreIndexableItem);

            if (siteInfo == null) return null;

            return this.GetBlacklistedSiteNames().Contains(siteInfo.Name, StringComparer.InvariantCultureIgnoreCase)
                ? Definitions.Constants.Markers.GlobalSiteMarker
                : this.ResolveDependency<ISanitizer>()?.SanitizeSiteName(siteInfo.Name);
        }

        protected virtual SiteInfo GetSiteInfo(Item item)
        {
            if (item == null) return null;

            // TODO: Check if this doesn't change default Sitecore site resolving behaviour (order)
            return SiteContextFactory.Sites
                .Where(s => !string.IsNullOrWhiteSpace(s.RootPath) && item.Paths.Path.StartsWith(s.RootPath, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.RootPath.Length)
                .FirstOrDefault();
        }

        protected virtual IEnumerable<string> GetBlacklistedSiteNames()
        {
            return new[]
            {
                Constants.AdminSiteName,
                Constants.ModulesShellSiteName,
                Constants.PublishingSiteName,
                Constants.ShellSiteName
            };
        }
    }
}