namespace Unic.UrlMapper2.Pipelines.HttpRequestBegin
{
    using Sitecore.Annotations;
    using Sitecore.Pipelines.HttpRequest;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class TryPerformRedirect : ProcessorBase<HttpRequestArgs>
    {
        protected override bool ShouldExecute(HttpRequestArgs args) => base.ShouldExecute(args) && this.ResolveDependency<IUrlMapperContext>()?.Item == null;

        protected override void Execute(HttpRequestArgs args)
        {
            var redirectSearchData = this.ResolveDependency<IRedirectSearchDataService>().GetDefaultRedirectSearchData(args);
            if (redirectSearchData == null) return;

            var redirectionService = this.ResolveDependency<IRedirectionService>();
            redirectionService?.PerformRedirect(redirectSearchData, args.HttpContext);
        }
    }
}