. ./scripts/Write-Log.ps1

function Invoke-DockerBuild {
    [CmdletBinding()]
    Param(
    )
    Process {
      
		Write-Log "Starting Docker Compose Build..."
        docker-compose build
		Write-Log "Finished Docker Compose Build"
    }
}