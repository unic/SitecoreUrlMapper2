namespace Unic.UrlMapper2.Services
{
    using Sitecore.Data;
    using Unic.UrlMapper2.Definitions;

    public class SpecificationService : ISpecificationService
    {
        public string GetSharedSpecification(Database database, ID specificationItemId)
        {
            var item = database.GetItem(specificationItemId);
            if (item == null)
            {
                // TODO: Add logging
                return null;
            }

            var value = item[Constants.Fields.Specification.Value];
            if (string.IsNullOrWhiteSpace(value))
            {
                // TODO: Add logging
            }

            return value;
        }
    }
}