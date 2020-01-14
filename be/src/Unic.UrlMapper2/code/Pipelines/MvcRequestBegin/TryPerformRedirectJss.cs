namespace Unic.UrlMapper2.Pipelines.MvcRequestBegin
{
    using Sitecore.Annotations;
    using Sitecore.LayoutService.Mvc.Routing;
    using Sitecore.Mvc.Pipelines.Request.RequestBegin;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class TryPerformRedirectJss : UrlMapperProcessorBase<RequestBeginArgs>
    {
        protected override bool ShouldExecute(RequestBeginArgs args)
        {
            var context = this.ResolveDependency<IUrlMapperContext>();
            return base.ShouldExecute(args)
                   && context?.Item is null
                   && context?.Site != null
                   && (this.ResolveDependency<IRouteMapper>()?.IsLayoutServiceRoute(args.RequestContext) ?? false);
        }

        protected override void Execute(RequestBeginArgs args)
        {
            var httpContext = args.RequestContext.HttpContext;

            var redirectSearchData = this.ResolveDependency<IRedirectSearchDataService>().GetJssRedirectSearchData(httpContext);
            if (redirectSearchData is null) return;

            var redirectionService = this.ResolveDependency<IRedirectionService>();
            redirectionService?.PerformRedirect(redirectSearchData, httpContext);
        }
    }
}