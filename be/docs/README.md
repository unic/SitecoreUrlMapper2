# Getting Started

## Software Versions

This version of the website uses the following software versions:

| Software      | Version |
| ---           | --- |
| Sitecore      | 9.2.0 |
| Solr          | 7.5.0 |
| Sitecore Installation Framework | 2.1.0 |
| Visual Studio / MSBuild | 2019 / 16.x |

## Locations and settings

This project assumes the following settings:

| Setting                          |  Value                                                                             | Change in <sup>1 *see below*</sup> |
| ---                              | ---                                                                                | --- |
| Website location                 | C:\inetpub\wwwroot\urlmapper2.dev.local\                                           | `settings.ps1`, `LocalSettings.props.user` |
| Website URL (Portal)             | [http://urlmapper2.dev.local/](http://urlmapper2.dev.local/)                       | `settings.ps1`, `Project.Portal.Sites.config` |
| SQL Server                       | .                                                                                  | `settings.ps1` |
| SQL Server Admin                 | sa                                                                                 | `settings.ps1` |
| SQL Server Password              | sa                                                                                 | `settings.ps1` |
| SOLR URL                         | [https://localhost:62200/solr](https://localhost:62200/solr) (*Note https*)        | `settings.ps1` |

## Prerequisites

**Important!: Check the prerequisites before starting the installation.**

* **Do check** the prerequisites of Sitecore Experience Platform in the release notes available on [dev.sitecore.net](https://dev.sitecore.net)
* **Do check** the [Resources](./Resources.md) page for the tools needed
* **Always** run your Visual Studio or PowerShell Command Line with elevated privileges or *As Administrator*

The Sitecore install script will check some prerequisites, including running the SIF `Prerequisites.json` configuration.

### Solr

The local installation requires the Apache Solr search engine. 
Solr must be running in a docker container and be available at [https://localhost:62200/solr](https://localhost:62200/solr) with a valid self-signed certificate.

All of that is handled for you when you run the `Initialize-DockerEnvironment` task in the *Task Runner Explorer* within Visual Studio.

> After running the task, [verify that Solr is running](https://localhost:62200/solr). Be aware, at this point **no Solr cores have been created**. The will be created by SIF during the installation.

## Installation

### 1. Installing Sitecore

1. **Clone or Download** the repository to your local file system.
1. Download the correct version of Sitecore from [dev.sitecore.net](https://dev.sitecore.net/Downloads.aspx) and place it in the `.\build\assets` folder.
    * This project will install by default on an *Sitecore XP Single*, i.e. a standalone version of Sitecore CMS including xConnect.
    * The currently supported version is defined in the `.\settings.ps1` file
    * The installation requires the following files:
        * A valid Sitecore license: `.\build\assets\license.xml`
        * Sitecore package: `.\build\assets\Sitecore X.X.X rev. XXXXXX (OnPrem)_single.scwdp.zip`
        * xConnect package: `.\build\assets\Sitecore X.X.X rev. XXXXXX (OnPrem)_xp0xconnect.scwdp.zip`
        * Identity Server package: `.\build\assets\Sitecore.IdentityServer X.X.X rev. XXXXXX (OnPrem)_identityserver.scwdp.zip`
1. Download the Sitecore Azure Toolkit from [dev.sitecore.net](https://dev.sitecore.net/Downloads/Sitecore_Azure_Toolkit.aspx) and extract the contents of the `tools` folder into `/build/sitecore-azure-toolkit`
1. Download the correct version of JSS and Sitecore PowerShell extensions and place them into the prepared /build/sitecore-modules folders. Check the README.mds within the subfolders for additional information about naming and the required versions. After that, run the `build\sitecore-modules\Prepare-WebDeployPackages.ps1`. This script will prepare the .scwdps so they can be installed on a local instance.
1. Are you using system settings other than the defaults specified at the top of this page?
    * If yes, you need to update the files accordingly.
    * **Include or omit trailing slashes as per the default setting in each file!**
1. Open an elevated privileges PowerShell command prompt (started with **Run as administrator**)
1. Run **`.\install-xp0.ps1`**
    * On subsequent runs of the install, you may use the `-SkipPrerequisites` flag to skip prerequisite checks.

        > After executing the **`.\install-xp0.ps1`** script you might run into this error: `The service cannot accept control messages at this time` (the issue is that the IIS app pool is getting targeted, while it's still restarting). If this happens, go to `.\build\sif-configs\XP0-SingleDeveloper.json` and delete the tasks before `"SitecoreModules"` under `"Includes"`. Now just run the script again. **Don't forget to revert your changes on the .json file**

install-xp0.ps1:146

1. Create a file `.\be\LocalSettings.props.user` with the following content:

```xml
<Project>
    <PropertyGroup>
        <WebRootPath>c:\inetpub\wwwroot\urlmapper2.dev.local</WebRootPath>
    </PropertyGroup>
</Project>
```

1. Ensure that the path in `<WebRootPath>` corresponds to your local web root path  
**!!! Changes to `LocalSettings.props.user` require a Visual Studio restart !!!**
1. Build the solution in Visual Studio  
*If you encounter a build failure caused by locked assemblies in the webroot, run an `iisreset` and try again.*

### 3. Run a full Unicorn sync

1. [Sync Unicorn](http://urlmapper2.dev.local/unicorn.aspx?verb=sync&log=Debug&skipTransparentConfigs=false) for the first time and check for sync warnings and errors.  
*The `Sync all` link might not appear on an initial sync. Don't worry, you can use the link provided above directly*

### 4. Rebuild search indexes

1. [Rebuild](https://doc.sitecore.net/sitecore_experience_platform/setting_up_and_maintaining/search_and_indexing/indexing/rebuild_search_indexes) all search indexes through the *Indexing manager* in the *Control Panel*.  
*This is required for search-based features to work properly*

### 5. Install JSS assets

1. Run the `Install-Frontend` task from within Visual Studio's Task Runner Explorer

### Helper tasks (in Visual Studio)

* The `Initialize-DockerEnvironment` task invokes a docker build, pulls all the latest images, runs *docker-compose up* and installs all self-signed certificates into the local store.
* The `Start-DockerEnvironment` task pulls all the latest images and runs *docker-compose up*
* The `Install-Frontend` task will build the JSS app and deploy the generated assets into the webroot and to the headless proxy directory (and replace the urls within the assets in order to work with a headless setup)

## Using Azure Search for local development

Please follow these steps if you want to connect to Azure Search instead of SolR:

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

## Available URLs and redirects

The solution has the following two sites configured:

- http://urlmapper2.dev.local - this is the jss site
- http://urlmapper2.dev.local/integration - this is the "integration" site and is not running in JSS

The docker setup also provides a SEQ container used for logging, which can be accessed through the following url: http://localhost:62100/#/events

The development environments ships with some pre-defined redirects you can use for local testing. You can find them in the following .csv (which also acts as an example of the `.csv` import format): `be\etc\import\example-import-file.csv`

## Creating a release

Don't forget to update the `AssemblyVersion` and `AssemblyFileVersion` in `AssemblyInfo.cs` to reflect the current release number and trigger a build so the updated assemblies are used for the installation package.

When you have made changes to the module and you want to disitribute those, there is a handy Sitecore PowerShell script you can use to generate the installation packages. The script can be found here: `/sitecore/system/Modules/PowerShell/Script Library/UrlMapper2/Development/Generate-DistributionPackage`. Don't forget to set the proper version in the `$version` variable.

Once you execute this script, it will prompt you to download two separate files: One for the CM and one for the CD instance.

### Converting the installation packages to .scwdps

Using the [Sitecore Azure Toolkit](https://dev.sitecore.net/Downloads/Sitecore_Azure_Toolkit.aspx), you can easily convert the generated packages to `.scwdp`s which you can then use in your ARM/Azure deployments. Use the `ConvertTo-SCModuleWebDeployPackage` cmdlet as follows:

```powershell
ConvertTo-SCModuleWebDeployPackage "<path-to-installation-package>\UrlMapper2_CD-<version>.zip" "<output-path>"
ConvertTo-SCModuleWebDeployPackage "<path-to-installation-package>\UrlMapper2_CM-<version>.zip" "<output-path>"
```

> Packages generated by Sitecore Azure Toolkit don't really follow the Microsoft standards in the `parameters.xml` file, since they define a `Application Path` parameter containing the application name instead of the standard `IIS Web Application Name` parameters. In order to make the packages work with the default IIS Deployment and Azure App Service deployment tasks, the `parameters.xml` needs to be adjusted accordingly and injected into the packages.

You can adjust the `parameters.xml` using the `Update-SCWebDeployPackage` cmdlet as follows:
```powershell
Update-SCWebDeployPackage -Path "<path-to-scwdp>" -ParametersXmlPath "<path-to-parameters-xml>"
```

Please note that if you are planning to use the modules for local or on-prem installations, you will have to disable dacpac options, which can be accomplished as follows:

```powershell
Update-SCWebDeployPackage -Path "<path-to-scwdp>" -DisableDacPacOptions '*' -ParametersXmlPath "<path-to-parameters-xml>"
```

More information about package conversion can be found in the [official documentation](https://doc.sitecore.com/developers/sat/20/sitecore-azure-toolkit/en/web-deploy-packages-for-a-module.html).

