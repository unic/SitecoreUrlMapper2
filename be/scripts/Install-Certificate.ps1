. ./scripts/Write-Log.ps1

function Get-Certificate {
    [CmdletBinding()]
    Param(
        [string] $certName
    )
    Process {
        return Get-ChildItem Cert:\LocalMachine\Root | Where-Object FriendlyName -eq "$certName"
    }
}

function Install-Certificate {
    [CmdletBinding()]
    Param(
        [string] $Name,
		[string] $HostName = "localhost"
    )
    Process {
        $certName = $Name
        $certStorePath = "./etc/docker/certs/$certName-ssl.keystore.pfx"
        $hostName = $HostName

        if (!(Get-Certificate "$certName")) {
            Write-Log "Trusting an new SSL Cert for $hostName ($certName)"

            $cert = Get-ChildItem $certStorePath
            Import-PfxCertificate -CertStoreLocation cert:\LocalMachine\Root -FilePath $certStorePath
        }
        else {
            Write-Log "SSL Cert $hostName ($certName) already exsist"
        }
    }
}
