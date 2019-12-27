namespace Unic.UrlMapper2.Models
{
    public class RedirectSearchData
    {
        public string SourceTerm { get; }

        public string Language { get; }

        public string SiteName { get; }

        public string EmbeddedLanguage { get; }

        public RedirectSearchData(
            string sourceTerm,
            string language,
            string siteName,
            string embeddedLanguage)
        {
            this.SourceTerm = sourceTerm;
            this.Language = language;
            this.SiteName = siteName;
            this.EmbeddedLanguage = embeddedLanguage;
        }
    }
}