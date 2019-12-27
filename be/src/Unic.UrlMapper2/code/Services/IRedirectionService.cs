namespace Unic.UrlMapper2.Services
{
    using Unic.UrlMapper2.Models;

    public interface IRedirectionService
    {
        void PerformRedirect(RedirectSearchData redirectSearchData);
        
        string SanitizeTerm(string value);
    }
}