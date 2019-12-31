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

1. Run the `Insatll-Frontend` task from within Visual Studio's Task Runner Explorer

### Helper tasks (in Visual Studio)

* The `Initialize-DockerEnvironment` task invokes a docker build, pulls all the latest images, runs *docker-compose up* and installs all self-signed certificates into the local store.
* The `Start-DockerEnvironment` task pulls all the latest images and runs *docker-compose up*
* The `Insatll-Frontend` task will build the JSS app and deploy the generated assets into the webroot and to the headless proxy directory (and replace the urls within the assets in order to work with a headless setup)