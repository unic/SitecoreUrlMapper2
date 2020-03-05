function Write-Log {
    [CmdletBinding()]
    Param(
        [string] $message
    )
    Process {
		$timestamp = "[{0:MM/dd/yy} {0:HH:mm:ss}]" -f (Get-Date)
        return Write-Host  $timestamp $message
    }
}