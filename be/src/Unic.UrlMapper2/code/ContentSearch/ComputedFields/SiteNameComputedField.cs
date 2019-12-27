namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;
    using Sitecore.Sites;
    using Sitecore.Web;

    [Sitecore.Annotations.UsedImplicitly]
    public class SiteNameComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public virtual object ComputeFieldValue(IIndexable indexable)
        {
            var siteInfo = this.GetSiteInfo(indexable as SitecoreIndexableItem);

            if (siteInfo == null) return null;

            return this.GetBlacklistedSiteNames().Contains(siteInfo.Name, StringComparer.InvariantCultureIgnoreCase)
                ? Definitions.Constants.Markers.GlobalSiteMarker
                : siteInfo.Name.ToLower();
        }

        protected virtual SiteInfo GetSiteInfo(Item item)
        {
            if (item == null) return null;

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