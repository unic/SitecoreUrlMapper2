namespace Unic.UrlMapper2.Definitions
{
    using Sitecore.Data;

    public static class Constants
    {
        public struct Markers
        {
            public const string GlobalSiteMarker = "any";
        }

        public struct FieldNames
        {
            public const string SourceTerm = "Source Term";

            public const string TargetUrl = "Target Url";
        }

        public struct Templates
        {
            public static readonly ID Redirect = ID.Parse("{91B6672E-51A2-48DB-9A17-D9E8744EE490}");

            public static readonly ID RedirectShared = ID.Parse("{DBDEE63A-98BC-4BF0-84EF-D955329FE680}");
        }

        public struct Settings
        {
            public const string ActiveIndex = "UrlMapper2.ActiveIndex";
        }
    }
}