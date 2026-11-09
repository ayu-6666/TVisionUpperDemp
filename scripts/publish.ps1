[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("net6.0-windows", "net8.0-windows")]
    [string]$Framework,
    [string]$Runtime = "win-x64",
    [string]$Configuration = "Release",
    [string]$Version = "0.1.0"
)

$ErrorActionPreference = "Stop"
$project = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot "..\TVisionUpperDemp.csproj"))
$artifactRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot "..\artifacts"))
$publishDir = Join-Path $artifactRoot "publish\$Framework-$Runtime"
$packageName = "TVisionUpperDemp-$Framework-$Runtime"
$packagePath = Join-Path $artifactRoot "$packageName.zip"

if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
if (Test-Path $packagePath) { Remove-Item $packagePath -Force }
New-Item -ItemType Directory -Path $artifactRoot -Force | Out-Null

Write-Host "Publishing $Framework for $Runtime ($Configuration), version $Version..."
$publishArgs = @(
    "publish", $project,
    "--configuration", $Configuration,
    "--framework", $Framework,
    "--runtime", $Runtime,
    "--self-contained", "false",
    "--output", $publishDir,
    "-p:Version=$Version"
)
dotnet @publishArgs
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE" }

Compress-Archive -Path (Join-Path $publishDir "*") -DestinationPath $packagePath -CompressionLevel Optimal
Write-Host "Package created: $packagePath"
