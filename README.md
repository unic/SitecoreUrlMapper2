# Sitecore URL Mapper 2

Sitecore URL Mapper 2 is a Sitecore Module allowing Authors to define redirects within Sitecore. This module is the successor to our [original Url Mapper Module](https://github.com/unic/SitecoreUrlMapper).

## Features

### Support for Plain Sitecore and JSS Headless

The module fully supports JSS in integrated and headless mode. Of course, native Sitecore setups are also supported.

In order to support redirects in JSS, you will have to patch the following processor within the `<mvc.requestBegin>` pipeline:

```xml
<processor patch:after="*[last()]" type="Unic.UrlMapper2.Pipelines.MvcRequestBegin.TryPerformRedirectJss, Unic.UrlMapper2" />
```

### Support for Multi-Site setups

Redirects can be defined on Site or a Global level, allowing you to target only specific sites or all sites within your setup.

### Wildcard and Regex matching

Redirects can be defined with Regex patterns, allowing you to be even more specific about what should and should not be redirected.

For example, if you would like to have a "wildcard" matching, you can define the term as `^mytest`, which will result in all of the following requests being a match: `https://mysite.com/test`, `https://mysite.com/testing`, `https://mysite.com/test-redirect`.

### Regex Captures

Capture groups can be defined when the regex feature has been enabled for a redirect. Capture groups can be used for example for transfering incoming query strings to the defined target.

The following term definition matches incoming terms for `global-capture` and transfers all query strings to the target (if any have been provided): `^global-capture([?].*)?`

### Preserve Query String

When this checkbox is checked, the query string from the original request will be passed on to the redirect location. When `Regex enabled` is unchecked, the request url path (excluding the query) has to be an exact match for the `Source term`. If `Source Term` contains a query, then the request url's query string has to start with the query from the `Source Term`.

For example, when `Preserve Query String` is checked, `Regex enabled` is unchecked, and `Source Term` is set to `test`, then this request: `https://mysite.com/test?a=b` will be redirected to the target with the query string included.
If the `Source Term` would be `test?a=b` then this request: `https://mysite.com/test?a=b&c=d` will be a match and redirected to the target with the whole query string included, but this request: `https://mysite.com/test?c=d&a=b` will not be a match.

### Bulk Import

A Sitecore PowerShell script is included allowing Authors to upload a CSV containing redirect definitions.

The csv format looks as follows: `Type;Name;Site;TargetSite;Permanent;Regex;Protocol;LanguageCode;Term;Target;Description`

|Column|Description|
|---|---|
|Type|Defines on what level the redirect should be created. The value can be either `global` or `site`|
|Name|Item name of the redirect. This value will be used to determine what language versions belong to this item.|
|Site|Sitecore Site name, used if the redirect type is set to `site`. The redirect will then be created within that sites redirect folder|
|TargetSite|If you are creating cross-site links, or global links, this value determines in what site the item target item will be searched in. This value needs to be internal Sitecore site name|
|Permanent|Defines if the redirect should be created as a permanent redirect. Set to `x` to enable permanent redirect|
|Regex|Defines if the redirect should support regex patterns within the term field. Set to `x` to enable regex support|
|Protocol|Used to set the `Protocol` field on the item. Can be either `http`, `https` or `any`|
|LanguageCode|Determines the language version of the record to be created|
|Term|Defines the term that should be matched|
|Target|Defines the target of the redirect. This can be either an external URL (eg. `https://google.com`), or the path to an item. Please note that the path shall not include any language code or site name (if you are using virtual paths). Example: `/products/super-product`|
|Description|An optional description of the redirect. This will be fed into the `Description` field on the created redirect item|
|PreserveQueryString|Set to `x` to handle and pass along query string from the requested URL|

A set of example csv records can be found here: `be\etc\import\example-import-file.csv`

### Easy installation, prepared for Azure and Azure DevOps

The Module can be downloaded either from Github or the Sitecore Marketplace. The module is distributed as a standard Sitecore installation package and also as Sitecore Web Deploy package (scwdp), allowing it to be included into existing ARM templates or referenced through the out of the box Azure DevOps Web Deploy tasks.

### Support for SolR and Azure Search

URL Mapper is a ContentSearch driven module. It ships with pre-defined configurations and support for SolR and Azure Search.

### Extensibility

The module offers several extension points and the framework elements have been developed in a way that they can be easily overwritten and adapted for custom requirements.

## Pre-Requisites and limitations

- The Content Search Provider must be either SolR or Azure Search, Lucene is not supported.
- The module assumes that Sitecore PowerShell extensions are available.

## Development

If you would like to extend the Module and set up a local development environment, please follow the steps described [over here](/be/docs/README.md).