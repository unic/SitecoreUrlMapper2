namespace Unic.UrlMapper2.Services
{
    using Sitecore;
    using Unic.UrlMapper2.Models;

    public class Sanitizer : ISanitizer
    {
        public virtual void SanitizeRedirectSearchData(RedirectSearchData redirectSearchData)
        {
            redirectSearchData.SourceTerm = this.SanitizeTerm(redirectSearchData.SourceTerm);
            redirectSearchData.SiteName = this.SanitizeSiteName(redirectSearchData.SiteName);
            redirectSearchData.SourceProtocol = this.SanitizeProtocol(redirectSearchData.SourceProtocol);
            redirectSearchData.Language = this.SanitizeLanguage(redirectSearchData.Language);
        }

        public virtual string SanitizeTerm(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;

            value = value.Trim().ToLower();

            value = StringUtil.RemovePrefix('/', value);
            value = StringUtil.RemovePostfix('/', value);

            return value;
        }

        public virtual string SanitizeSiteName(string value) => value?.Trim().ToLower();

        public virtual string SanitizeProtocol(string value) => value?.Trim().ToLower();

        protected virtual string SanitizeLanguage(string value) => value?.Trim().ToLower();
    }
}