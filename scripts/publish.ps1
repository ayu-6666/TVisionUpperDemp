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

if (Test-Path $publishDir) {
    Remove-Item $publishDir -Recurse -Force
}
if (Test-Path $packagePath) {
    Remove-Item $packagePath -Force
}
New-Item -ItemType Directory -Path $artifactRoot -Force | Out-Null

# Convert RELEASE-V1.0.0 / v1.0.0 to 1.0.0.
$Version = $Version -replace '^(?i:RELEASE-)', ''
$Version = $Version -replace '^[vV]', ''
if ($Version -notmatch '^\d+\.\d+\.\d+(\.\d+)?(-[0-9A-Za-z.-]+)?(\+[0-9A-Za-z.-]+)?$') {
    throw "Invalid version '$Version'. Expected a version such as 1.0.0."
}

Write-Host "Restoring $Framework for $Runtime..."
$restoreArgs = @(
    "restore"
    $project
    "-p:TargetFrameworks=$Framework"
    "-p:TargetFramework=$Framework"
    "-p:RuntimeIdentifier=$Runtime"
    "-p:SelfContained=true"
    "-p:Version=$Version"
)
& dotnet @restoreArgs
if ($LASTEXITCODE -ne 0) {
    throw "dotnet restore failed with exit code $LASTEXITCODE"
}

Write-Host "Publishing self-contained $Framework for $Runtime ($Configuration), version $Version..."
$publishArgs = @(
    "publish"
    $project
    "--configuration"
    $Configuration
    "--framework"
    $Framework
    "--runtime"
    $Runtime
    "--self-contained"
    "true"
    "--no-restore"
    "--output"
    $publishDir
    "-p:TargetFrameworks=$Framework"
    "-p:TargetFramework=$Framework"
    "-p:RuntimeIdentifier=$Runtime"
    "-p:Version=$Version"
)
& dotnet @publishArgs
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE"
}

if (!(Test-Path (Join-Path $publishDir "TVisionUpperDemp.exe"))) {
    throw "Publish output is missing TVisionUpperDemp.exe: $publishDir"
}

Compress-Archive -Path (Join-Path $publishDir "*") -DestinationPath $packagePath -CompressionLevel Optimal
if (!(Test-Path $packagePath)) {
    throw "ZIP package was not created: $packagePath"
}

Write-Host "Package created: $packagePath"
