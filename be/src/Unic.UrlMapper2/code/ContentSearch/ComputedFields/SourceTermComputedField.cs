namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.Annotations;
    using Sitecore.ContentSearch;
    using Unic.UrlMapper2.Services;
    using Constants = Definitions.Constants;

    [UsedImplicitly]
    public class SourceTermComputedField : UrlMapperComputedFieldBase
    {
        protected override object Compute(IIndexable indexable)
        {
            var value = indexable?.GetFieldByName(Constants.Fields.Redirect.SourceTerm)?.Value as string;

            return string.IsNullOrWhiteSpace(value)
                ? default
                : this.ResolveDependency<ISanitizer>()?.SanitizeTerm(value);
        }
    }
}