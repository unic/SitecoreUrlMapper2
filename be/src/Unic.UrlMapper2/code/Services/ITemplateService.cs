namespace Unic.UrlMapper2.Services
{
    using Sitecore.Data;

    public interface ITemplateService
    {
        ID GetRedirectTemplateId();

        bool IsRedirectTemplate(ID templateId);
    }
}