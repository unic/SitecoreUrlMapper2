namespace Unic.UrlMapper2.Services
{
    using Sitecore.Data;
    using Unic.UrlMapper2.Definitions;

    public class TemplateService : ITemplateService
    {
        public virtual ID GetRedirectTemplateId() => Constants.Templates.Redirect;

        public bool IsRedirectTemplate(ID templateId) => templateId == this.GetRedirectTemplateId();
    }
}