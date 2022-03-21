namespace Unic.UrlMapper2.Definitions
{
    using Sitecore.Data;

    public static class Constants
    {
        public struct Markers
        {
            public const string GlobalSiteMarker = "any";

            public const string AnyProtocolMarker = "any";
        }

        public struct Fields
        {
            public struct Redirect
            {
                public const string SourceTerm = "Source Term";

                public const string Target = "Target";

                public const string RedirectType = "Redirect Type";

                public const string SourceProtocol = "Source Protocol";

                public const string RegexEnabled = "Regex enabled";

                public const string PreserveQueryString = "Preserve Query String";
            }

            public struct Specification
            {
                public const string Value = "Value";
            }
        }

        public struct Templates
        {
            public static readonly ID Redirect = ID.Parse("{91B6672E-51A2-48DB-9A17-D9E8744EE490}");
        }

        public struct Settings
        {
            public const string ActiveIndex = "UrlMapper2.ActiveIndex";

            public const string UseProtocolHeaderForDefaultProcessor = "UrlMapper2.UseProtocolHeaderForDefaultProcessor";

            public const string ProtocolHeaderForDefaultProcessor = "UrlMapper2.ProtocolHeaderForDefaultProcessor";

            public const string UseProtocolHeaderForJssProcessor = "UrlMapper2.UseProtocolHeaderForJssProcessor";

            public const string ProtocolHeaderForJssProcessor = "UrlMapper2.ProtocolHeaderForJssProcessor";

            public const string UseOriginalUrlHeaderForJssProcessor = "UrlMapper2.UseOriginalUrlHeaderForJssProcessor";

            public const string OriginalUrlHeaderForJssProcessor = "UrlMapper2.OriginalUrlHeaderForJssProcessor";
        }

        public struct RegularExpressions
        {
            public const string QueryStringExpression = "([?].*)?";
        }
    }
}