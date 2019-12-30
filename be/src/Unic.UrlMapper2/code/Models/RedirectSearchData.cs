namespace Unic.UrlMapper2.Models
{
    public class RedirectSearchData
    {
        public string SourceTerm { get; set;  }

        public string Language { get; set;  }

        public string SiteName { get; set;  }

        public string EmbeddedLanguage { get; set;  }

        public bool ContainsEmbeddedLanguage => !string.IsNullOrWhiteSpace(this.EmbeddedLanguage);

        public string SourceProtocol { get; set; }

        public RedirectSearchData(
            string sourceTerm,
            string language,
            string siteName,
            string embeddedLanguage,
            string sourceProtocol)
        {
            this.SourceTerm = sourceTerm;
            this.Language = language;
            this.SiteName = siteName;
            this.EmbeddedLanguage = embeddedLanguage;
            this.SourceProtocol = sourceProtocol;
        }
    }
}