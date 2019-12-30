namespace Unic.UrlMapper2.Services
{
    using Sitecore.Pipelines.HttpRequest;
    using Unic.UrlMapper2.Models;

    public interface IRedirectSearchDataService
    {
        RedirectSearchData GetDefaultRedirectSearchData(HttpRequestArgs args);
    }
}
