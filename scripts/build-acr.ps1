<#
.SYNOPSIS
    Builds Docker images using Azure Container Registry (ACR) task functionality.
.DESCRIPTION
    Uses 'az acr build' to submit build contexts to Azure Container Registry,
    which builds the images on the cloud without requiring Docker locally.
    
    Prerequisites:
    - Azure CLI (az) installed and authenticated (az login)
    - Azure Container Registry created
    
.PARAMETER AcrName
    Name of the Azure Container Registry (e.g. 'myregistry' for myregistry.azurecr.io)
    
.PARAMETER ResourceGroup
    Azure Resource Group containing the ACR (optional - ACR name is globally unique)
    
.PARAMETER BackendTag
    Tag for the backend image (default: 'latest')
    
.PARAMETER FrontendTag
    Tag for the frontend image (default: 'latest')
    
.PARAMETER ViteApiBaseUrl
    The API base URL to bake into the frontend image (default: 'http://localhost:5066')
    This is passed as a build argument to the frontend Dockerfile.
    
.PARAMETER NoPush
    Build only, do not push to registry.
    
.EXAMPLE
    .\build-acr.ps1 -AcrName "myregistry"
    
.EXAMPLE
    .\build-acr.ps1 -AcrName "myregistry" -BackendTag "v1.0.0" -FrontendTag "v1.0.0" -ViteApiBaseUrl "https://api.myapp.com"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$AcrName,
    
    [Parameter(Mandatory = $false)]
    [string]$ResourceGroup = "",
    
    [Parameter(Mandatory = $false)]
    [string]$BackendTag = "latest",
    
    [Parameter(Mandatory = $false)]
    [string]$FrontendTag = "latest",
    
    [Parameter(Mandatory = $false)]
    [string]$ViteApiBaseUrl = "http://localhost:5066",
    
    [Parameter(Mandatory = $false)]
    [switch]$NoPush
)

$ErrorActionPreference = "Stop"

# Verify az CLI is available
if (-not (Get-Command "az" -ErrorAction SilentlyContinue)) {
    Write-Error "Azure CLI (az) is not installed. Install from: https://aka.ms/installazurecliwindows"
    exit 1
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir

# Image names
$backendImage = "ase-enterprise-api:$BackendTag"
$frontendImage = "vue-search-app:$FrontendTag"

# ACR registry argument
$registryArg = if ($ResourceGroup) { "--resource-group $ResourceGroup" } else { "" }

Write-Host "🏗️  Azure Container Registry Build" -ForegroundColor Cyan
Write-Host "   Registry:  $AcrName.azurecr.io" -ForegroundColor Gray
Write-Host "   Backend:   $backendImage" -ForegroundColor Gray
Write-Host "   Frontend:  $frontendImage" -ForegroundColor Gray
Write-Host ""

# --- Build Backend ---
Write-Host "🔧 Building backend image..." -ForegroundColor Yellow
$backendContext = Join-Path $repoRoot "src\AgentScratchEnterprise"
$backendDockerfile = "ASE.EnterpriseApi\Dockerfile"

$backendArgs = @(
    "acr", "build",
    "--registry", $AcrName,
    "--image", $backendImage,
    "--file", $backendDockerfile
)
if ($ResourceGroup) { $backendArgs += @("--resource-group", $ResourceGroup) }
if ($NoPush) { $backendArgs += "--no-push" }
$backendArgs += $backendContext

Write-Host "   Context: $backendContext" -ForegroundColor Gray
Write-Host "   Command: az $($backendArgs -join ' ')" -ForegroundColor DarkGray
az @backendArgs

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Backend build failed!"
    exit 1
}
Write-Host "✅ Backend image built successfully." -ForegroundColor Green
Write-Host ""

# --- Build Frontend ---
Write-Host "⚛️  Building frontend image..." -ForegroundColor Yellow
$frontendContext = Join-Path $repoRoot "src\chat-web-app"

$frontendArgs = @(
    "acr", "build",
    "--registry", $AcrName,
    "--image", $frontendImage,
    "--build-arg", "VITE_API_BASE_URL=$ViteApiBaseUrl"
)
if ($ResourceGroup) { $frontendArgs += @("--resource-group", $ResourceGroup) }
if ($NoPush) { $frontendArgs += "--no-push" }
$frontendArgs += $frontendContext

Write-Host "   Context: $frontendContext" -ForegroundColor Gray
Write-Host "   VITE_API_BASE_URL: $ViteApiBaseUrl" -ForegroundColor Gray
Write-Host "   Command: az $($frontendArgs -join ' ')" -ForegroundColor DarkGray
az @frontendArgs

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Frontend build failed!"
    exit 1
}
Write-Host "✅ Frontend image built successfully." -ForegroundColor Green
Write-Host ""

Write-Host "🎉 All images built successfully!" -ForegroundColor Cyan
if (-not $NoPush) {
    Write-Host ""
    Write-Host "Images available at:" -ForegroundColor Gray
    Write-Host "   $AcrName.azurecr.io/$backendImage" -ForegroundColor White
    Write-Host "   $AcrName.azurecr.io/$frontendImage" -ForegroundColor White
}
