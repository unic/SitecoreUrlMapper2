namespace Unic.UrlMapper2.Pipelines.MvcRequestBegin
{
    using Sitecore.Annotations;
    using Sitecore.LayoutService.Mvc.Routing;
    using Sitecore.Mvc.Pipelines.Request.RequestBegin;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class TryPerformRedirectJss : ProcessorBase<RequestBeginArgs>
    {
        protected override bool ShouldExecute(RequestBeginArgs args) => 
            base.ShouldExecute(args)
            && this.ResolveDependency<IContext>()?.Item == null
            && (this.ResolveDependency<IRouteMapper>()?.IsLayoutServiceRoute(args.RequestContext) ?? false);

        protected override void Execute(RequestBeginArgs args)
        {
            var httpContext = args.RequestContext.HttpContext;

            var redirectSearchData = this.ResolveDependency<IRedirectSearchDataService>().GetJssRedirectSearchData(httpContext);
            if (redirectSearchData == null) return;

            var redirectionService = this.ResolveDependency<IRedirectionService>();
            redirectionService?.PerformRedirect(redirectSearchData, httpContext);
        }
    }
}