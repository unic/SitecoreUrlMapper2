namespace Unic.UrlMapper2.Models
{
    public class RedirectSearchData
    {
        public string SourceTerm { get; set; }

        /// <summary>
        /// Source Term with the original casing
        /// </summary>
        public string SourceTermOriginal { get; set; }

        public string Language { get; set; }

        public string SiteName { get; set; }

        public string SourceProtocol { get; set; }

        public RedirectSearchData(
            string sourceTerm,
            string sourceTermOriginal,
            string language,
            string siteName,
            string sourceProtocol)
        {
            this.SourceTerm = sourceTerm;
            this.SourceTermOriginal = sourceTermOriginal;
            this.Language = language;
            this.SiteName = siteName;
            this.SourceProtocol = sourceProtocol;
        }
    }
}