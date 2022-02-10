namespace Unic.UrlMapper2.Services
{
    using Sitecore;
    using Unic.UrlMapper2.Models;

    public class Sanitizer : ISanitizer
    {
        public virtual void SanitizeRedirectSearchData(RedirectSearchData redirectSearchData)
        {
            redirectSearchData.SourceTerm = this.SanitizeTerm(redirectSearchData.SourceTerm);
            redirectSearchData.SourceTermOriginal = this.SanitizeTerm(redirectSearchData.SourceTermOriginal, false);
            redirectSearchData.SiteName = this.SanitizeSiteName(redirectSearchData.SiteName);
            redirectSearchData.SourceProtocol = this.SanitizeProtocol(redirectSearchData.SourceProtocol);
        }

        public virtual string SanitizeTerm(string value, bool withToLower = true)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;

            value = value.Trim();

            value = StringUtil.RemovePrefix('/', value);
            value = StringUtil.RemovePostfix('/', value);

            return withToLower ? value.ToLower() : value;
        }

        public virtual string SanitizeSiteName(string value) => value?.Trim().ToLower();

        public virtual string SanitizeProtocol(string value) => value?.Trim().ToLower();
    }
}