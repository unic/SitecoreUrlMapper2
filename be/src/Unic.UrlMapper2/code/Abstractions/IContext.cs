namespace Unic.UrlMapper2.Abstractions
{
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.Sites;

    public interface IContext
    {
        SiteContext Site { get; set; }

        Item Item { get; set; }

        Language Language { get; set; }

        Language FilePathLanguage { get; set; }
    }
}