Set-StrictMode -Version 2.0

Register-SitecoreInstallExtension -Command Expand-Archive -As ExpandArchive -Type Task
Register-SitecoreInstallExtension -Command Remove-Item -As Remove -Type Task