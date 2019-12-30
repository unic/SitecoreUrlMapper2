namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Unic.UrlMapper2.DependencyInjection;

    public abstract class ComputedFieldBase : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public abstract object ComputeFieldValue(IIndexable indexable);

        protected virtual T ResolveDependency<T>() where T : class => Container.Resolve<T>();
    }
}