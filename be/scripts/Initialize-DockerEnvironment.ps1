. ./scripts/Invoke-DockerBuild.ps1
. ./scripts/Start-DockerEnvironment.ps1
. ./scripts/Install-Certificate.ps1

# run the build to ensure we have all docker images
Invoke-DockerBuild

# pull all the latest images and start up the environment
Start-DockerEnvironment

# make sure the self-signed certificates are trusted
Install-Certificate -Name urlmapper2-solr