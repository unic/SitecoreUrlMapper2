. ./scripts/Write-Log.ps1

function Start-DockerEnvironment {
    [CmdletBinding()]
    Param(
    )
    Process {

        # Always attempt to pull a newer version of the image
		Write-Log "Starting Docker Pull..."
        docker-compose pull --ignore-pull-failures
		Write-Log "Finished Docker Pull"

		Write-Log "Starting Docker Up..."
        docker-compose up -d
		Write-Log "Finished Docker Up"

    }
}