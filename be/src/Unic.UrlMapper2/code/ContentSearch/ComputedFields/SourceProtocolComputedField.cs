namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.Abstractions;
    using Sitecore.Annotations;
    using Sitecore.ContentSearch;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Unic.UrlMapper2.Definitions;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class SourceProtocolComputedField : UrlMapperComputedFieldBase
    {
        protected override object Compute(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);

            var value = indexable?.GetFieldByName(Constants.Fields.Redirect.SourceProtocol)?.Value;
            if (ID.TryParse(value, out var specificationId))
            {
                var protocol = this.ResolveDependency<ISpecificationService>().GetSharedSpecification(item.Database, specificationId);
                return this.ResolveDependency<ISanitizer>().SanitizeProtocol(protocol);
            }

            this.ResolveDependency<BaseLog>().Error("Failed to determine source protocol", this);

            return null;
        }
    }
}