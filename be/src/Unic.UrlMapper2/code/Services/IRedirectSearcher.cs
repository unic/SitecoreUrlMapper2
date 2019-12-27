namespace Unic.UrlMapper2.Services
{
    using System.Collections.Generic;
    using Unic.UrlMapper2.Models;

    public interface IRedirectSearcher
    {
        IEnumerable<Redirect> SearchRedirects(RedirectSearchData redirectSearchData);
    }
}