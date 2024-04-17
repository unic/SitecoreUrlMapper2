# escape=`

# This Dockerfile will build the Sitecore solution and save the build artifacts for use in
# other images, such as 'cm' and 'rendering'. It does not produce a runnable image itself.

ARG BUILD_IMAGE

# In a separate image (as to not affect layer cache), gather all NuGet-related solution assets, so that
# we have what we need to run a cached NuGet restore in the next layer:
# https://stackoverflow.com/questions/51372791/is-there-a-more-elegant-way-to-copy-specific-files-using-docker-copy-to-the-work/61332002#61332002
# This technique is described here:
# https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1#the-dockerfile-1
FROM ${BUILD_IMAGE} AS nuget-prep
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
COPY be/*.sln be/nuget.config be/Directory.Build.targets be/Directory.Build.props /nuget/
COPY be/src/ /temp/
RUN Invoke-Expression 'robocopy C:/temp C:/nuget/src /s /ndl /njh /njs *.csproj *.scproj '


FROM ${BUILD_IMAGE} AS builder
ARG BUILD_CONFIGURATION
ARG NUGET_USER
ARG NUGET_PAT

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Ensure updated nuget. Depending on your Windows version, dotnet/framework/sdk:4.8 tag may provide an outdated client.
# See https://github.com/microsoft/dotnet-framework-docker/blob/1c3dd6638c6b827b81ffb13386b924f6dcdee533/4.8/sdk/windowsservercore-ltsc2019/Dockerfile#L7
ENV NUGET_VERSION 6.2.0

RUN Invoke-WebRequest "https://dist.nuget.org/win-x86-commandline/v$env:NUGET_VERSION/nuget.exe" -UseBasicParsing -OutFile "$env:ProgramFiles\NuGet\nuget.exe"

WORKDIR /build

# Copy prepped NuGet artifacts, and restore as distinct layer to take advantage of caching.
COPY --from=nuget-prep ./nuget ./

# Restore NuGet packages
RUN nuget restore -Verbosity quiet

# Copy remaining source code
COPY be/src/ ./src/

# Copy transforms, retaining directory structure
RUN Invoke-Expression 'robocopy C:\build\src C:\out\transforms /s /ndl /njh /njs *.xdt'

# Build the Sitecore main platform artifacts
RUN msbuild .\src\Unic.UrlMapper2\code\Unic.UrlMapper2.csproj /p:Configuration=release /p:DeployOnBuild=True /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=False /p:publishUrl="c:\out\module"
# Save the artifacts for copying into other images (see 'cm' and 'rendering' Dockerfiles).

FROM mcr.microsoft.com/windows/nanoserver:1809
WORKDIR /artifacts

COPY --from=builder C:\out\module\App_Config\Include\UrlMapper2\ .\module\App_Config\Include\UrlMapper2\
COPY --from=builder C:\out\module\bin\MoreLinq.dll .\module\bin\
COPY --from=builder C:\out\module\bin\Unic.UrlMapper2.dll .\module\bin\