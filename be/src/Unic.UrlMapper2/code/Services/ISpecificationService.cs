namespace Unic.UrlMapper2.Services
{
    using Sitecore.Data;

    public interface ISpecificationService
    {
        string GetSharedSpecification(Database database, ID specificationItemId);
    }
}