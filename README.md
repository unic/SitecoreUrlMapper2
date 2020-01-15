# Sitecore URL Mapper 2

Sitecore URL Mapper 2 is a Sitecore Module allowing Authors to define redirects within Sitecore. This module is the successor to our [original Url Mapper Module](https://github.com/unic/SitecoreUrlMapper).

## Features

### Support for Plain Sitecore and JSS Headless

The module fully supports JSS in integrated and headless mode. Of course, native Sitecore setups are also supported.

### Support for Multi-Site setups

Redirects can be defined on Site or a Global level, allowing you to target only specific sites or all sites within your setup.

### Wildcard and Regex matching

Redirects can be defined with Regex patterns, allowing you to be even more specific about what should and should not be redirected.

### Bulk Import

A Sitecore PowerShell script is included allowing Authors to upload a CSV containing redirect definitions.

### Easy installation, prepared for Azure and Azure DevOps

The Module can be downloaded either from Github or the Sitecore Marketplace. The module is distributed as a standard Sitecore installation package and also as Sitecore Web Deploy package (scwdp), allowing it to be included into existing ARM templates or referenced through the out of the box Azure DevOps Web Deploy tasks.

### Support for SolR and Azure Search

URL Mapper is a ContentSearch driven module. It ships with pre-defined configurations and support for SolR and Azure Search.

### Extensibility

The module offers several extension points and the framework elements have been developed in a way that they can be easily overwritten and adapted for custom requirements.

## Pre-Requisites and limitations

- The Content Search Provider must be either SolR or Azure Search, Lucene is not supported.
- The module assumes that Sitecore PowerShell extensions are available.