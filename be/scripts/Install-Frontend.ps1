$projectPath = Join-Path $PSScriptRoot "..\.."
$dockerImage = 'urlmapper-fe-build'

Write-Host "Building docker image (using cache)"
docker build -t $dockerImage $projectPath\"fe\urlmapper-jss-app"

Write-Host "Building frontend assets and templates"
$dockerContainer = docker run -d $dockerImage
# Output docker logs although container is running detached
docker logs -f $dockerContainer

Write-Host "Copying frontend jss app to web root"
$webRoot = Select-Xml -Path $projectPath\be\LocalSettings.props.user -XPath "/Project/PropertyGroup/WebRootPath" | Select-Object -expand Node | Select-Object -expand '#text'
$jssTargetPath = "$webRoot\dist\urlmapper-jss-app"

if (!(Test-Path $jssTargetPath)) {
    New-Item $jssTargetPath -ItemType Directory
}

docker cp ${dockerContainer}:/app/build/. $jssTargetPath

Write-Host "Copying frontend jss app to headless proxy"
$headlessTargetPath = "$projectPath\fe\node-headless-ssr-proxy\dist\urlmapper-jss-app"

if (!(Test-Path $headlessTargetPath)) {
    New-Item $headlessTargetPath -ItemType Directory
}

docker cp ${dockerContainer}:/app/build/. $headlessTargetPath

Write-Host "Cleaning up docker container"
docker rm $dockerContainer -f

Write-Host "Replacing URLs for headless proxy"
Get-ChildItem $headlessTargetPath -File -recurse | ForEach {
    (Get-Content -Path $_.FullName).Replace("http://urlmapper2.dev.local", "http://localhost:3000") | Set-Content -Path $_.FullName
}