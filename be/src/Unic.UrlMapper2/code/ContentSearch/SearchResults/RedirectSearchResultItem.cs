namespace Unic.UrlMapper2.ContentSearch.SearchResults
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Converters;
    using Sitecore.Data;

    public class RedirectSearchResultItem
    {
        [IndexField("_latestversion")]
        public bool IsLatestVersion { get; set; }

        [IndexField("_template")]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        [DataMember]
        public virtual ID TemplateId { get; set; }

        [IndexField("_language")]
        [DataMember]
        public virtual string Language { get; set; }

        [IndexField("_group")]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        [DataMember]
        public virtual ID ItemId { get; set; }

        [IndexField("urlmapper_source_term")]
        [DataMember]
        public virtual string SourceTerm { get; set; }

        [IndexField("urlmapper_target_url")]
        [DataMember]
        public virtual string TargetUrl { get; set; }

        [IndexField("urlmapper_site_name")]
        [DataMember]
        public virtual string SiteName { get; set; }

        [IndexField("urlmapper_redirect_type")]
        [DataMember]
        public virtual string RedirectType { get; set; }

        [IndexField("urlmapper_source_protocol")]
        [DataMember]
        public virtual string SourceProtocol { get; set; }

        [IndexField("urlmapper_include_embedded_language")]
        [DataMember]
        public virtual bool IncludeEmbeddedLanguage { get; set; }
    }
}