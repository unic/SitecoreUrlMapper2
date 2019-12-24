# Sitecore Install Framework (SIF) Configuration Files

These are the default config files with minor adaptions:

- Solr start and stop tasks have been removed from `sitecore-solr.json` and `xconnect-solr.json`
- The `Solr.Server` variable has been changed to map directly to the root of the solr cores `[variable('Solr.FullRoot')]`

The changes are necessary because we don't run solr as a Windows Service, but in a Docker container.

Additionally, the following changes have been added:

- The `SitecoreModules` config has been included to install Sitecore modules through `sitecore-modules.json`