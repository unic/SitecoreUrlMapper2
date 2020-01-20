namespace Unic.UrlMapper2.Models
{
    public class Redirect
    {
        public string TargetUrl { get; set; }

        public RedirectType RedirectType { get; set; }

        public SourceProtocol SourceProtocol { get; set; }

        public RedirectSearchData RedirectSearchData { get; set; }
        
        public bool RegexEnabled { get; set; }

        public string Term { get; set; }
    }
}