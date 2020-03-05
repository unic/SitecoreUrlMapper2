namespace Unic.UrlMapper2.Abstractions
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.Sites;

    public class UrlMapperContext : IUrlMapperContext
    {
        public SiteContext Site
        {
            get => Sitecore.Context.Site;
            set => Sitecore.Context.Site = value;
        }

        public Item Item
        {
            get => Sitecore.Context.Item;
            set => Sitecore.Context.Item = value;
        }

        public Language Language
        {
            get => Sitecore.Context.Language;
            set => Sitecore.Context.Language = value;
        }

        public Database Database
        {
            get => Sitecore.Context.Database;
            set => Sitecore.Context.Database = value;
        }
    }
}