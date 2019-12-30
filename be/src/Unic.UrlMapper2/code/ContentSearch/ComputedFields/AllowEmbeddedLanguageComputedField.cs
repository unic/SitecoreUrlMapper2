namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore;
    using Sitecore.ContentSearch;
    using Constants = Definitions.Constants;

    [Sitecore.Annotations.UsedImplicitly]
    public class AllowEmbeddedLanguageComputedField : ComputedFieldBase
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            var value = indexable?.GetFieldByName(Constants.Fields.Redirect.AllowEmbeddedLanguage)?.Value;
            return MainUtil.GetBool(value, false);
        }
    }
}