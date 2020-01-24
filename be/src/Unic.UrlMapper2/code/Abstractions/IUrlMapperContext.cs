namespace Unic.UrlMapper2.Abstractions
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.Sites;

    public interface IUrlMapperContext
    {
        SiteContext Site { get; set; }

        Item Item { get; set; }

        Language Language { get; set; }

        Database Database { get; set; }
    }
}