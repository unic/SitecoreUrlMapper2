namespace Unic.UrlMapper2.Services
{
    using Sitecore.Data;
    using Unic.UrlMapper2.Definitions;

    public class TemplateService : ITemplateService
    {
        public virtual ID GetRedirectTemplateId() => Constants.Templates.Redirect;

        public virtual ID GetSharedRedirectTemplateId() => Constants.Templates.SharedRedirect;

        public bool IsRedirectTemplate(ID templateId) => templateId == this.GetRedirectTemplateId() || templateId == this.GetSharedRedirectTemplateId();
    }
}