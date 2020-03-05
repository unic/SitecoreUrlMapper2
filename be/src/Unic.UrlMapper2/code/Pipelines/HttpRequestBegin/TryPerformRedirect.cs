namespace Unic.UrlMapper2.Pipelines.HttpRequestBegin
{
    using Sitecore.Annotations;
    using Sitecore.Pipelines.HttpRequest;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class TryPerformRedirect : UrlMapperProcessorBase<HttpRequestArgs>
    {
        protected override bool ShouldExecute(HttpRequestArgs args)
        {
            var context = this.ResolveDependency<IUrlMapperContext>();
            return base.ShouldExecute(args) && context?.Item is null && context?.Site != null;
        }

        protected override void Execute(HttpRequestArgs args)
        {
            var redirectSearchData = this.ResolveDependency<IRedirectSearchDataService>().GetDefaultRedirectSearchData(args);
            if (redirectSearchData is null) return;

            var redirectionService = this.ResolveDependency<IRedirectionService>();
            redirectionService?.PerformRedirect(redirectSearchData, args.HttpContext);
        }
    }
}