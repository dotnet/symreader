[CmdletBinding(PositionalBinding=$false)]
Param(
  [string] $configuration = "Debug",
  [string] $solution = "",
  [string] $verbosity = "minimal",
  [switch] $restore,
  [switch] $build,
  [switch] $rebuild,
  [switch] $test,
  [switch] $sign,
  [switch] $pack,
  [switch] $ci,
  [switch] $prepareMachine,
  [switch] $log,
  [switch] $help,
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

set-strictmode -version 2.0
$ErrorActionPreference = "Stop"

function Print-Usage() {
    Write-Host "Common settings:"
    Write-Host "  -configuration <value>  Build configuration Debug, Release"
    Write-Host "  -verbosity <value>      Msbuild verbosity (q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic])"
    Write-Host "  -help                   Print help and exit"
    Write-Host ""

    Write-Host "Actions:"
    Write-Host "  -restore                Restore dependencies"
    Write-Host "  -build                  Build solution"
    Write-Host "  -rebuild                Rebuild solution"
    Write-Host "  -test                   Run all unit tests in the solution"
    Write-Host "  -sign                   Sign build outputs"
    Write-Host "  -pack                   Package build outputs into NuGet packages and Willow components"
    Write-Host ""

    Write-Host "Advanced settings:"
    Write-Host "  -solution <value>       Path to solution to build"
    Write-Host "  -ci                     Set when running on CI server"
    Write-Host "  -log                    Enable logging (by default on CI)"
    Write-Host "  -prepareMachine         Prepare machine for CI run"
    Write-Host ""
    Write-Host "Command line arguments not listed above are passed thru to msbuild."
    Write-Host "The above arguments can be shortened as much as to be unambiguous (e.g. -co for configuration, -t for test, etc.)."
}

if ($help -or (($properties -ne $null) -and ($properties.Contains("/help") -or $properties.Contains("/?")))) {
  Print-Usage
  exit 0
}

$InVSEnvironment = !($env:VS150COMNTOOLS -eq $null) -and (Test-Path $env:VS150COMNTOOLS)
$RepoRoot = Join-Path $PSScriptRoot "..\"
$DotNetRoot = Join-Path $RepoRoot ".dotnet"
$DotNetExe = Join-Path $DotNetRoot "dotnet.exe"
$ToolsRoot = Join-Path $RepoRoot ".tools"
$BuildProj = Join-Path $PSScriptRoot "build.proj"
$DependenciesProps = Join-Path $PSScriptRoot "Versions.props"
$ArtifactsDir = Join-Path $RepoRoot "artifacts"
$LogDir = Join-Path (Join-Path $ArtifactsDir $configuration) "log"
$TempDir = Join-Path (Join-Path $ArtifactsDir $configuration) "tmp"

function Create-Directory([string[]] $path) {
  if (!(Test-Path -path $path)) {
    New-Item -path $path -force -itemType "Directory" | Out-Null
  }
}

function GetDotNetCliVersion {
  [xml]$xml = Get-Content $DependenciesProps
  return $xml.Project.PropertyGroup.DotNetCliVersion
}

function InstallDotNetCli {
  
  Create-Directory $DotNetRoot
  $dotnetCliVersion = GetDotNetCliVersion

  $installScript="https://raw.githubusercontent.com/dotnet/cli/release/2.0.0/scripts/obtain/dotnet-install.ps1"
  Invoke-WebRequest $installScript -OutFile "$DotNetRoot\dotnet-install.ps1"
  
  & "$DotNetRoot\dotnet-install.ps1" -Version $dotnetCliVersion -InstallDir $DotNetRoot
  if ($lastExitCode -ne 0) {
    throw "Failed to install dotnet cli (exit code '$lastExitCode')."
  }
}

function GetVSWhereVersion {
  [xml]$xml = Get-Content $DependenciesProps
  return $xml.Project.PropertyGroup.VSWhereVersion
}

function LocateVisualStudio {
  if ($InVSEnvironment) {
    return Join-Path $env:VS150COMNTOOLS "..\.."
  }

  $vswhereVersion = GetVSWhereVersion
  $vsWhereDir = Join-Path $ToolsRoot "vswhere\$vswhereVersion"
  $vsWhereExe = Join-Path $vsWhereDir "vswhere.exe"
  
  if (!(Test-Path $vsWhereExe)) {
    Create-Directory $vsWhereDir   
    Write-Host "Downloading vswhere"
    Invoke-WebRequest "http://github.com/Microsoft/vswhere/releases/download/$vswhereVersion/vswhere.exe" -OutFile $vswhereExe
  }
  
  $vsInstallDir = & $vsWhereExe -latest -property installationPath -requires Microsoft.Component.MSBuild -requires Microsoft.VisualStudio.Component.VSSDK -requires Microsoft.Net.Component.4.6.TargetingPack -requires Microsoft.VisualStudio.Component.Roslyn.Compiler -requires Microsoft.VisualStudio.Component.VSSDK
  
  if (!(Test-Path $vsInstallDir)) {
    throw "Failed to locate Visual Studio (exit code '$lastExitCode')."
  }

  return $vsInstallDir
}

function Build {
  $vsInstallDir = LocateVisualStudio
  $msbuildExe = Join-Path $vsInstallDir "MSBuild\15.0\Bin\msbuild.exe"


  if ($ci -or $log) {
    Create-Directory($logDir)
    $logCmd = "/bl:" + (Join-Path $LogDir "Build.binlog")
  } else {
    $logCmd = ""
  }

  if ($ci) {
    Write-Host "Using $msbuildExe"
  }

  $nodeReuse = !$ci

  & $msbuildExe $BuildProj /m /nologo /clp:Summary /nodeReuse:$nodeReuse /v:$verbosity $logCmd /p:Configuration=$configuration /p:SolutionPath=$solution /p:Restore=$restore /p:Build=$build /p:Rebuild=$rebuild /p:Test=$test /p:Sign=$sign /p:Pack=$pack /p:CIBuild=$ci $properties
}

function Stop-Processes() {
  Write-Host "Killing running build processes..."
  Get-Process -Name "msbuild" -ErrorAction SilentlyContinue | Stop-Process
  Get-Process -Name "vbcscompiler" -ErrorAction SilentlyContinue | Stop-Process
}

try {
  if ($ci) {
    Create-Directory $TempDir
    $env:TEMP = $TempDir
    $env:TMP = $TempDir
  }

  if ($restore) {
    InstallDotNetCli
  }

  Build
  exit $lastExitCode
}
catch {
  Write-Host $_
  Write-Host $_.Exception
  Write-Host $_.ScriptStackTrace
  exit 1
}
finally {
  Pop-Location
  if ($ci -and $prepareMachine) {
    Stop-Processes
  }
}

