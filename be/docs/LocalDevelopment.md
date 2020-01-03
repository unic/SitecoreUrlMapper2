# Local Development

## Using Azure Search for local development

1) Create an Azure Cognitive Search service within the Azure Portal (make sure that you have at least the Standard plan selected)
1) Copy one of the admin keys
1) Add the following entry to the ConnectionStrings.config:
`<add name="cloud.search" connectionString="serviceUrl=<url>;apiVersion=<apiVersion>;apiKey=<apiKey>" /> `
1) Make sure to replace the tokens with the correct values. The `apiVersion` must be set to `2017-11-11`.
1) In the `web.config` change the search provider from Solr to Azure: `<add key="search:define" value="Azure" />`
1) Rebuild the indexes
1) Ensure that the Search Service within the Azure Portal now contains entries

Resources:
- [Configure Azure Search](https://doc.sitecore.com/developers/92/platform-administration-and-architecture/en/configure-azure-search.html)
- [Configure a search and indexing provider](https://doc.sitecore.com/developers/92/platform-administration-and-architecture/en/configure-a-search-and-indexing-provider.html)

## Using JSS

A sample component has been scaffolded and can be found in the `/fe` folder of the solution root. The API key used for the Layout service has been serialized for the integration website and is as follows: `{F02CC0B5-BF7B-41B0-B570-C398A59FEC19}`.
All required content for the jss app (such as renderings, placeholder settings, etc.) have been serialized. Additionally, the jss-specific Sitecore patchings have been included into the integration project. Therefore, you should be able to have a running JSS solution as soon as you have triggered a build, run the `Install-Frontend` task and have performed a Unicorn sync.

Additionally, a headless proxy based on the [official documentation](https://jss.sitecore.com/docs/techniques/ssr/headless-mode-ssr) has been added and configured in the `/fe/node-headless-ssr-proxy` folder.

In order to start the headless proxy, run the following commands in the `/fe/node-headless-ssr-proxy` directory: `npm install` and then `npm start`.
