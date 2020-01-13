namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;
    using Unic.UrlMapper2.DependencyInjection;
    using Unic.UrlMapper2.Services;

    public abstract class UrlMapperComputedFieldBase : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable) => this.ShouldCompute(indexable)
            ? this.Compute(indexable)
            : null;

        protected abstract object Compute(IIndexable indexable);

        protected virtual bool ShouldCompute(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);

            return this.ResolveDependency<ITemplateService>().IsRedirectTemplate(item.TemplateID);
        }

        protected virtual T ResolveDependency<T>() where T : class => Container.Resolve<T>();
    }
}