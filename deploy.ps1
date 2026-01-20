#Requires -Version 5.1
<#
.SYNOPSIS
    One-Click Deploy to Production Server

.DESCRIPTION
    Builds and deploys ImageProcessor to a Windows production server via SSH/SCP.

.PARAMETER Server
    Server hostname or IP address

.PARAMETER User
    SSH username

.PARAMETER Path
    Remote deployment path (default: C:\Deploy\ImageProcessor)

.PARAMETER Port
    SSH port (default: 22)

.PARAMETER SkipBuild
    Skip the build step (deploy existing build)

.EXAMPLE
    .\deploy.ps1 -Server 192.168.1.100 -User administrator
    Build and deploy to server

.EXAMPLE
    .\deploy.ps1 -Server myserver.com -User deploy -SkipBuild
    Deploy without rebuilding
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$Server,

    [Parameter(Mandatory=$true)]
    [string]$User,

    [string]$Path = 'C:\Deploy\ImageProcessor',

    [int]$Port = 22,

    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'

# Configuration
$ProjectName = 'ImageProcessor'
$PublishDir = 'publish\win-x64'
$RemotePath = $Path -replace '\\', '/'  # Convert to Unix-style for SCP
$RemotePath = "/$(($RemotePath -replace ':', '').ToLower())"  # C:\Deploy -> /c/Deploy

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "One-Click Deploy: $ProjectName" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Server:  $User@$Server" -ForegroundColor White
Write-Host "Path:    $Path" -ForegroundColor White
Write-Host "Port:    $Port" -ForegroundColor White
Write-Host ""

# Step 1: Build (unless skipped)
if (-not $SkipBuild) {
    Write-Host "[1/4] Building project..." -ForegroundColor Yellow
    & "$PSScriptRoot\build.ps1" -Configuration Release
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Build failed!" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "[1/4] Skipping build (using existing)" -ForegroundColor Yellow
}

# Verify build exists
if (-not (Test-Path "$PublishDir\$ProjectName.exe")) {
    Write-Host "ERROR: Build not found at $PublishDir\$ProjectName.exe" -ForegroundColor Red
    Write-Host "Run without -SkipBuild to create a new build" -ForegroundColor Yellow
    exit 1
}

# Step 2: Create remote directory
Write-Host ""
Write-Host "[2/4] Preparing remote directory..." -ForegroundColor Yellow
$sshResult = ssh -p $Port "$User@$Server" "mkdir -p '$RemotePath'" 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to create remote directory!" -ForegroundColor Red
    Write-Host $sshResult -ForegroundColor Red
    exit 1
}

# Step 3: Stop running application
Write-Host ""
Write-Host "[3/4] Stopping existing application..." -ForegroundColor Yellow
ssh -p $Port "$User@$Server" "taskkill /F /IM $ProjectName.exe 2>nul || echo No running instance"

# Step 4: Deploy files
Write-Host ""
Write-Host "[4/4] Deploying files to server..." -ForegroundColor Yellow
$scpResult = scp -P $Port -r "$PublishDir\*" "${User}@${Server}:$RemotePath/" 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Deployment failed!" -ForegroundColor Red
    Write-Host $scpResult -ForegroundColor Red
    exit 1
}

# Success
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "Deployment completed successfully!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "Application deployed to: ${User}@${Server}:$Path" -ForegroundColor White
Write-Host ""
Write-Host "To start the application:" -ForegroundColor Cyan
Write-Host "  ssh ${User}@${Server} `"$Path\$ProjectName.exe`"" -ForegroundColor White
Write-Host ""
