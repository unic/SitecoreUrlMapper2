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

To connect the JSS app to the integration environment use these steps:
1) Run `jss setup` and provide the correct values
1) Run `jss deploy config`
1) Run `jss deploy app -c -d`. You might get an exception here, in that case please use the following workaround: https://kb.sitecore.net/articles/650791

Resources:
- [App Deployment](https://jss.sitecore.com/docs/getting-started/app-deployment)
- [Errors when importing JSS application on Azure](https://kb.sitecore.net/articles/650791)