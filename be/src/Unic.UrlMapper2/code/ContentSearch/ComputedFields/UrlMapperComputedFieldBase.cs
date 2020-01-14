namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;
    using Unic.UrlMapper2.DependencyInjection;
    using Unic.UrlMapper2.Services;

    public abstract class UrlMapperComputedFieldBase : IComputedIndexField
    {
        private Scope scope;

        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            if (this.ShouldCompute(indexable))
            {
                object result;

                using (this.scope = Container.CreateScope())
                {
                    result = this.Compute(indexable);
                }

                this.scope = null;

                return result;
            }

            return default;
        }

        protected abstract object Compute(IIndexable indexable);

        protected virtual bool ShouldCompute(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);

            return this.ResolveDependency<ITemplateService>().IsRedirectTemplate(item.TemplateID);
        }

        protected virtual TDependency ResolveDependency<TDependency>()
            where TDependency : class => this.scope != default ? this.scope.Resolve<TDependency>() : Container.Resolve<TDependency>();
    }
}