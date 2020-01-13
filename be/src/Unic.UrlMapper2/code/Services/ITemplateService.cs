namespace Unic.UrlMapper2.Services
{
    using Sitecore.Data;

    public interface ITemplateService
    {
        ID GetRedirectTemplateId();

        ID GetSharedRedirectTemplateId();

        bool IsRedirectTemplate(ID templateId);
    }
}