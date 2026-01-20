#Requires -Version 5.1
<#
.SYNOPSIS
    Build script for ImageProcessor application

.DESCRIPTION
    Builds and publishes the ImageProcessor Windows Forms application
    as a self-contained executable.

.PARAMETER Configuration
    Build configuration (Debug or Release). Default: Release

.PARAMETER Runtime
    Target runtime. Default: win-x64
    Options: win-x64, win-x86, win-arm64

.PARAMETER Clean
    Clean build output before building

.PARAMETER Package
    Create a ZIP package after building

.EXAMPLE
    .\build.ps1
    Build with default settings

.EXAMPLE
    .\build.ps1 -Configuration Debug -Clean
    Clean build in Debug mode

.EXAMPLE
    .\build.ps1 -Package
    Build and create ZIP package
#>

param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [ValidateSet('win-x64', 'win-x86', 'win-arm64')]
    [string]$Runtime = 'win-x64',

    [switch]$Clean,

    [switch]$Package
)

$ErrorActionPreference = 'Stop'

# Configuration
$ProjectName = 'ImageProcessor'
$SolutionFile = 'ImageProcessor.sln'
$PublishDir = 'publish'
$ProjectFile = "$ProjectName\$ProjectName.csproj"

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Building $ProjectName" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Configuration: $Configuration"
Write-Host "Runtime: $Runtime"
Write-Host ""

# Clean if requested
if ($Clean -and (Test-Path $PublishDir)) {
    Write-Host "[0/4] Cleaning previous build..." -ForegroundColor Yellow
    Remove-Item -Path $PublishDir -Recurse -Force
}

# Restore
Write-Host "[1/4] Restoring dependencies..." -ForegroundColor Yellow
dotnet restore $SolutionFile
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Restore failed!" -ForegroundColor Red
    exit 1
}

# Build
Write-Host ""
Write-Host "[2/4] Building solution..." -ForegroundColor Yellow
dotnet build $SolutionFile --configuration $Configuration --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed!" -ForegroundColor Red
    exit 1
}

# Publish
Write-Host ""
Write-Host "[3/4] Publishing self-contained executable..." -ForegroundColor Yellow
$OutputPath = Join-Path $PublishDir $Runtime

dotnet publish $ProjectFile `
    --configuration $Configuration `
    --runtime $Runtime `
    --self-contained true `
    --output $OutputPath `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Publish failed!" -ForegroundColor Red
    exit 1
}

# Create version file
Write-Host ""
Write-Host "[4/4] Creating version info..." -ForegroundColor Yellow
$VersionInfo = @"
Build Date: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
Configuration: $Configuration
Runtime: $Runtime
Machine: $env:COMPUTERNAME
"@
$VersionInfo | Out-File -FilePath (Join-Path $OutputPath 'version.txt') -Encoding UTF8

# Package if requested
if ($Package) {
    Write-Host ""
    Write-Host "Creating ZIP package..." -ForegroundColor Yellow
    $ZipName = "$ProjectName-$Runtime-$(Get-Date -Format 'yyyyMMdd').zip"
    $ZipPath = Join-Path $PublishDir $ZipName
    Compress-Archive -Path "$OutputPath\*" -DestinationPath $ZipPath -Force
    Write-Host "Package created: $ZipPath" -ForegroundColor Green
}

# Summary
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "Output directory: $OutputPath"
Write-Host ""
Write-Host "Files:" -ForegroundColor Cyan
Get-ChildItem $OutputPath | Format-Table Name, Length -AutoSize

# Return the output path
return $OutputPath
