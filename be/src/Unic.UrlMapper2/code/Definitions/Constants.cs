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

                public const string TargetUrl = "Target Url";

                public const string RedirectType = "Redirect Type";

                public const string SourceProtocol = "Source Protocol";

                public const string AllowEmbeddedLanguage = "Allow Embedded Language";

                public const string WildcardEnabled = "Wildcard enabled";
            }

            public struct Specification
            {
                public const string Value = "Value";
            }
        }

        public struct Templates
        {
            public static readonly ID Redirect = ID.Parse("{91B6672E-51A2-48DB-9A17-D9E8744EE490}");

            public static readonly ID SharedRedirect = ID.Parse("{DBDEE63A-98BC-4BF0-84EF-D955329FE680}");
        }

        public struct Settings
        {
            public const string ActiveIndex = "UrlMapper2.ActiveIndex";

            public const string UseProtocolHeaderForDefaultProcessor = "UrlMapper2.UseProtocolHeaderForDefaultProcessor";

            public const string ProtocolHeaderForDefaultProcessor = "UrlMapper2.ProtocolHeaderForDefaultProcessor";

            public const string UseProtocolHeaderForJssProcessor = "UrlMapper2.UseProtocolHeaderForJssProcessor";

            public const string ProtocolHeaderForJssProcessor = "UrlMapper2.ProtocolHeaderForJssProcessor";
        }
    }
}