namespace Unic.UrlMapper2.Pipelines.HttpRequestBegin
{
    using Sitecore.Annotations;
    using Sitecore.Pipelines.HttpRequest;
    using Unic.UrlMapper2.Abstractions;
    using Unic.UrlMapper2.Models;
    using Unic.UrlMapper2.Services;

    [UsedImplicitly]
    public class PerformRedirectIfPossible : ProcessorBase<HttpRequestArgs>
    {
        protected override bool ShouldExecute(HttpRequestArgs args) => base.ShouldExecute(args) && this.ResolveDependency<IContext>()?.Item == null;

        protected override void Execute(HttpRequestArgs args)
        {
            var redirectSearchData = this.GetRedirectSearchData(args);
            if (redirectSearchData == null) return;

            var redirectionService = this.ResolveDependency<IRedirectionService>();
            redirectionService?.PerformRedirect(redirectSearchData);
        }

        protected virtual RedirectSearchData GetRedirectSearchData(HttpRequestArgs args)
        {
            var context = this.ResolveDependency<IContext>();
            if (context == null) return null;

            return new RedirectSearchData(
                sourceTerm: args.LocalPath,
                language: context.Language?.Name,
                siteName: context.Site?.Name?.ToLower(),
                embeddedLanguage: context.FilePathLanguage?.Name);
        }
    }
}