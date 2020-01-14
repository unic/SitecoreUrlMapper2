namespace Unic.UrlMapper2.Services
{
    using Sitecore.Abstractions;
    using Sitecore.Data;
    using Unic.UrlMapper2.Definitions;

    public class SpecificationService : ISpecificationService
    {
        private readonly BaseLog logger;

        public SpecificationService(BaseLog logger)
        {
            this.logger = logger;
        }

        public string GetSharedSpecification(Database database, ID specificationItemId)
        {
            var item = database.GetItem(specificationItemId);
            if (item == null)
            {
                this.logger.Error($"Failed to get specification item with id {specificationItemId}", this);
                return null;
            }

            var value = item[Constants.Fields.Specification.Value];
            if (string.IsNullOrWhiteSpace(value))
            {
                this.logger.Error($"Specification item {item.ID} does not have a value.", this);
            }

            return value;
        }
    }
}