namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.Annotations;
    using Sitecore.ContentSearch;
    using Unic.UrlMapper2.Definitions;

    [UsedImplicitly]
    public class TargetUrlComputedField : ComputedFieldBase
    {
        public override object ComputeFieldValue(IIndexable indexable) => indexable?.GetFieldByName(Constants.Fields.Redirect.TargetUrl)?.Value;
    }
}