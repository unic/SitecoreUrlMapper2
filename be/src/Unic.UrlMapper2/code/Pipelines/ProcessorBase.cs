namespace Unic.UrlMapper2.Pipelines
{
    using System.Collections.Generic;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.DependencyInjection;
    using Unic.UrlMapper2.Services;

    public abstract class ProcessorBase<T>
    {
        protected List<string> AllowedSites { get; } = new List<string>();

        protected List<string> RestrictedSites { get; } = new List<string>();

        public virtual void Process(T args)
        {
            if (this.ShouldExecute(args)) this.Execute(args);
        }

        protected virtual bool ShouldExecute(T args)
        {
            var context = this.ResolveDependency<IUrlMapperContext>();
            var siteExecutionFilter = this.ResolveDependency<IUrlMapperSiteExecutionFilter>();

            return siteExecutionFilter.IsSiteAllowed(context?.Site, this.AllowedSites, this.RestrictedSites);
        }

        protected abstract void Execute(T args);

        protected virtual TDependency ResolveDependency<TDependency>()
            where TDependency : class => Container.Resolve<TDependency>();
    }
}