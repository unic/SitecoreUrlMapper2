namespace Unic.UrlMapper2.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public static class Container
    {
        public static T Resolve<T>()
            where T : class => ServiceLocator.ServiceProvider.GetRequiredService<T>();

        public static Scope CreateScope() =>
            new Scope(ServiceLocator.ServiceProvider.GetService<IServiceScopeFactory>().CreateScope());
    }
}