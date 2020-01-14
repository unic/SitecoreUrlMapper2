# Solution parameters
$SolutionPrefix = "urlmapper2"
$SitePostFix = "dev.local"
$Webroot = Join-Path $env:SystemDrive 'inetpub\wwwroot'

$SitecoreVersion = "9.2.0 rev. 002893"
$IdentityServerVersion = "3.0.0 rev. 00211"
$InstallerVersion = "2.1.0"

# Assets and prerequisites
$ConfigsRoot = "$PSScriptRoot\..\build\sif-configs"
$ModulesRoot = "$PSScriptRoot\..\build\sitecore-modules"
$DatabaseRoot = "$PSScriptRoot\..\build\database"
$AssetsRoot = "$PSScriptRoot\..\build\assets"
$AssetsPSRepository = "https://sitecore.myget.org/F/sc-powershell/api/v2/"
$AssetsPSRepositoryName = "SitecoreGallery"

$LicenseFile = "$PSScriptRoot\..\build\assets\license.xml"

# SQL Parameters
$SqlServer = "."
$SqlAdminUser = "sa"
$SqlAdminPassword = "sa"

# Prerequisities Check
$PrerequisitiesConfiguration = "$ConfigsRoot\Prerequisites.json"

# XP0 Single Developer Parameters
$SingleDeveloperConfiguration = "$ConfigsRoot\XP0-SingleDeveloper.json"

# Sitecore Parameters
$SitecorePackage = "$AssetsRoot\Sitecore $SitecoreVersion (OnPrem)_single.scwdp.zip"
$SitecoreSiteName = "$SolutionPrefix.$SitePostFix"
$SitecoreSiteUrl = "http://$SitecoreSiteName"
$SitecoreSiteRoot = Join-Path $Webroot -ChildPath $SitecoreSiteName
$SitecoreAdminPassword = "b"

# XConnect Parameters
$XConnectPackage = "$AssetsRoot\Sitecore $SitecoreVersion (OnPrem)_xp0xconnect.scwdp.zip"
$XConnectSiteName = "${SolutionPrefix}_xconnect.$SitePostFix"
$XConnectSiteUrl = "https://$XConnectSiteName"
$XConnectSiteRoot = Join-Path $Webroot -ChildPath $XConnectSiteName

# Identity Server Parameters
$IdentityServerSiteName = "${SolutionPrefix}_IdentityServer.$SitePostFix"
$IdentityServerUrl = "https://$IdentityServerName"
$IdentityServerPackage = "$AssetsRoot\Sitecore.IdentityServer $IdentityServerVersion (OnPrem)_identityserver.scwdp.zip"
$IdentityClientSecret = "SPDHZpF6g8EXq5F7C5EhPQdsC1UbvTU3"
$IdentityAllowedCorsOrigins = "$SitecoreSiteUrl"
$IdentityServerSiteRoot = Join-Path $Webroot -ChildPath $IdentityServerSiteName

# Solr Parameters
$SolrUrl = "https://localhost:62200/solr"
$SolrRoot = "$PSScriptRoot\etc\docker\solrdata"

# Modules Parameters
$JSSServerPackage = "$ModulesRoot\JSS\JSS_CM.scwdp.zip"
$PSEPackage = "$ModulesRoot\PSE\PSE_CM.scwdp.zip"