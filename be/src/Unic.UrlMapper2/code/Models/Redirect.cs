namespace Unic.UrlMapper2.Models
{
    using Sitecore.Data;

    public class Redirect
    {
        public RedirectType RedirectType { get; set; }

        public SourceProtocol SourceProtocol { get; set; }

        public RedirectSearchData RedirectSearchData { get; set; }
        
        public bool RegexEnabled { get; set; }

        public string Term { get; set; }

        public ID ItemId { get; set; }
    }
}