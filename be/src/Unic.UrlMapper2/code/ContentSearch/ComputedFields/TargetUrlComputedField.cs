namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.Annotations;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Unic.UrlMapper2.Definitions;

    [UsedImplicitly]
    public class TargetUrlComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public virtual object ComputeFieldValue(IIndexable indexable) => indexable?.GetFieldByName(Constants.FieldNames.TargetUrl)?.Value;
    }
}