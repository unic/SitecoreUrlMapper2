namespace Unic.UrlMapper2.DependencyInjection
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class Scope : IDisposable
    {
        private readonly IServiceScope scope;

        internal Scope(IServiceScope scope)
        {
            this.scope = scope;
        }

        public void Dispose()
        {
            this.scope.Dispose();
        }

        public T Resolve<T>() where T : class => this.scope.ServiceProvider.GetRequiredService<T>();
    }
}