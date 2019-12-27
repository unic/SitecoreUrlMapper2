namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Constants = Definitions.Constants;

    [Sitecore.Annotations.UsedImplicitly]
    public class SourceTermComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public virtual object ComputeFieldValue(IIndexable indexable)
        {
            var value = indexable?.GetFieldByName(Constants.FieldNames.SourceTerm)?.Value as string;

            return string.IsNullOrWhiteSpace(value)
                ? null
                : this.SanitizeValue(value);
        }

        protected virtual string SanitizeValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            value = StringUtil.RemovePrefix('/', value);
            value = StringUtil.RemovePostfix('/', value);

            return value;
        }
    }
}