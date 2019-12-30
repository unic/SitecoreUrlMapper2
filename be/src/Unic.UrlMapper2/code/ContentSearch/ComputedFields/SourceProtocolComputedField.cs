namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.Annotations;
    using Sitecore.ContentSearch;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Unic.UrlMapper2.Definitions;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class SourceProtocolComputedField : ComputedFieldBase
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);

            var value = indexable?.GetFieldByName(Constants.Fields.Redirect.SourceProtocol)?.Value;
            if (ID.TryParse(value, out var specificationId))
            {
                return this.ResolveDependency<ISpecificationService>().GetSharedSpecification(item.Database, specificationId);
            }

            // TODO: Add logging
            return null;
        }
    }
}