namespace Unic.UrlMapper2.Services
{
    using System.Web;
    using Unic.UrlMapper2.Models;

    public interface IRedirectionService
    {
        void PerformRedirect(RedirectSearchData redirectSearchData, HttpContextBase httpContext);
    }
}