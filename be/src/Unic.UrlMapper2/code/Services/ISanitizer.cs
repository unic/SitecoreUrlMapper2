﻿namespace Unic.UrlMapper2.Services
{
    using Unic.UrlMapper2.Models;

    public interface ISanitizer
    {
        void SanitizeRedirectSearchData(RedirectSearchData redirectSearchData);

        string SanitizeTerm(string value);

        string SanitizeSiteName(string value);

        string SanitizeProtocol(string value);
    }
}