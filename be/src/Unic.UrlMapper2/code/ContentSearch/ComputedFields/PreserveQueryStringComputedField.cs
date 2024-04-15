namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore;
    using Sitecore.ContentSearch;
    using Constants = Definitions.Constants;

    [Sitecore.Annotations.UsedImplicitly]
    public class PreserveQueryStringComputedField : UrlMapperComputedFieldBase
    {
        protected override object Compute(IIndexable indexable)
        {
            var value = indexable?.GetFieldById(Constants.Fields.Redirect.PreserveQueryString)?.Value;
            
            return MainUtil.GetBool(value, false);
        }
    }
}