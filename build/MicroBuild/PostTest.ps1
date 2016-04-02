Param
(
    [string]$BinariesDirectory,
    [string]$NuGetExePath,
    [string]$MyGetApiKey
)

try
{
    $exitCode = 0
    
	Write-Host "Uploading NuGet packages..."
	
	$nugetDir = Join-Path $BinariesDirectory "Nuget"
	$apiKey = (Get-Content $MyGetApiKey).Trim()
	$FeedName = "symreader-master"

	Get-ChildItem $nugetDir -Filter *.nupkg | Foreach-Object 
	{
	    $nupkg = $_.FullName
	
		Write-Host "  Uploading '$nupkg'"
		
	    & "$NuGetExePath" push "$nupkg" `
			-Source ("https://www.myget.org/F/{0}/api/v2/package -f $FeedName)`
			-ApiKey $apiKey `
			-NonInteractive `
			-Verbosity quiet
			
		if ($LastExitCode -ne 0)
		{
			Write-Error "Failed to upload NuGet package: $nupkg"
			$exitCode = 3
		}
    }

	Write-Host "Completed PostTest script with an exit code of '$exitCode'"
	
    exit $exitCode
}
catch [exception]
{
    Write-Error -Exception $_.Exception
    exit -1
}
