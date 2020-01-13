namespace Unic.UrlMapper2.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Annotations;
    using Sitecore.DependencyInjection;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUrlMapperContext, UrlMapperContext>();
            serviceCollection.AddTransient<IUrlMapperSiteExecutionFilter, UrlMapperSiteExecutionFilter>();
            serviceCollection.AddScoped<ISpecificationService, SpecificationService>();
            serviceCollection.AddTransient<ISanitizer, Sanitizer>();
            serviceCollection.AddTransient<IRedirectSearcher, RedirectSearcher>();
            serviceCollection.AddTransient<IRedirectSearchDataService, RedirectSearchDataService>();
            serviceCollection.AddTransient<IRedirectionService, RedirectionService>();
            serviceCollection.AddTransient<ITemplateService, TemplateService>();
        }
    }
}