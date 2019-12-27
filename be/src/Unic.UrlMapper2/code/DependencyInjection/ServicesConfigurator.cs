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
            serviceCollection.AddTransient<IContext, Context>();
            serviceCollection.AddTransient<ISiteExecutionFilter, SiteExecutionFilter>();

            serviceCollection.AddTransient<IRedirectSearcher, RedirectSearcher>();
            serviceCollection.AddTransient<IRedirectionService, RedirectionService>();
        }
    }
}