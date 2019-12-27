namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Unic.UrlMapper2.DependencyInjection;
    using Unic.UrlMapper2.Services;
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
                : this.ResolveDependency<IRedirectionService>()?.SanitizeTerm(value);
        }

        protected virtual T ResolveDependency<T>() where T : class => Container.Resolve<T>();
    }
}