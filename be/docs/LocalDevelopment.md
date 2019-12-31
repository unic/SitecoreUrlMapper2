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