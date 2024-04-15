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
                public static readonly ID SourceTerm = ID.Parse("{BF458D1E-59C8-4661-9AD0-67A95CB85785}");

                public static readonly ID Target = ID.Parse("{B64E1624-9590-4CAD-B3B6-802F4C794086}");

                public static readonly ID RedirectType = ID.Parse("{BB67D28D-EFF1-4A65-BD01-DE64AC6E9DA4}");

                public static readonly ID SourceProtocol = ID.Parse("{E453D759-43E3-4033-ADA8-245DDB56980D}");

                public static readonly ID RegexEnabled = ID.Parse("{A21DADDF-E5B1-4134-B236-9515D9153EF6}");

                public static readonly ID PreserveQueryString = ID.Parse("{A9D3D444-C009-4C90-B1A2-F8C25D28D084}");
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
            public const string QueryStringPattern = "([?].*)?";
            public const string PartialQueryStringPattern = "([&].*)?";
        }
    }
}