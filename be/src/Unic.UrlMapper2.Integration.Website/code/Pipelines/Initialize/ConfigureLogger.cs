namespace Unic.UrlMapper2.Integration.Website.Pipelines.Initialize
{
    using Serilog;
    using Sitecore.Abstractions;
    using Sitecore.Annotations;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines;
    using Unic.UrlMapper2.Integration.Website.Definitions;
    using Unic.UrlMapper2.Integration.Website.Logging.Serilog;

    [UsedImplicitly]
    public class ConfigureLogger
    {
        private readonly BaseSettings settings;

        public ConfigureLogger(BaseSettings settings)
        {
            this.settings = settings;
        }

        public void Process(PipelineArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            var seqIsEnabled = this.settings?.GetBoolSetting(Constants.SeqServerIsEnabled, false) ?? false;
            var serverUrl = this.settings?.GetSetting(Constants.SeqServerUrlSetting, Constants.SeqServerUrlDefaultSetting)
                            ?? Constants.SeqServerUrlDefaultSetting;

            var supplyContextMessage = this.settings?.GetBoolSetting(Constants.Log4NetSupplyContextMessageSetting, Constants.Log4NetSupplyContextMessageDefaultSetting)
                                       ?? Constants.Log4NetSupplyContextMessageDefaultSetting;

            var loggerConfiguration = new LoggerConfiguration();
            if (seqIsEnabled)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Seq(serverUrl);
            }

            loggerConfiguration = loggerConfiguration.WriteTo.Log4Net(supplyContextMessage: supplyContextMessage);
            global::Serilog.Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}