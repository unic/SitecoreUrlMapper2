Get-ChildItem -Path $PSScriptRoot/../sitecore-azure-toolkit/ | Unblock-File

Import-Module $PSScriptRoot/../sitecore-azure-toolkit/Sitecore.Cloud.Cmdlets.dll
Import-Module $PSScriptRoot/../sitecore-azure-toolkit/Sitecore.Cloud.Cmdlets.psm1

Update-SCWebDeployPackage -ParametersXmlPath $PSScriptRoot/JSS/parameters.xml -Path $PSScriptRoot/JSS/JSS_CM.scwdp.zip -DisableDacPacOptions '*'
Update-SCWebDeployPackage -ParametersXmlPath $PSScriptRoot/PSE/parameters.xml -Path $PSScriptRoot/PSE/PSE_CM.scwdp.zip -DisableDacPacOptions '*'

Write-Host "Done"