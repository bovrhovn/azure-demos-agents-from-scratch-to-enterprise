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
    
.PARAMETER BackendImage
    Full image name for the backend, optionally including a tag (e.g. 'ase-enterprise-api:v1.0.0').
    If no tag is specified the tag defaults to 'latest'.
    
.PARAMETER FrontendImage
    Full image name for the frontend, optionally including a tag (e.g. 'vue-search-app:v1.0.0').
    If no tag is specified the tag defaults to 'latest'.
    
.PARAMETER NoPush
    Build only, do not push to registry.
    
.NOTES
    VITE_API_BASE_URL is no longer a build argument. Set it as a runtime environment variable
    when running the container (e.g. -e VITE_API_BASE_URL=https://api.myapp.com).
    
.EXAMPLE
    .\build-acr.ps1 -AcrName "myregistry"
    
.EXAMPLE
    .\build-acr.ps1 -AcrName "myregistry" -BackendImage "ase-enterprise-api:v1.0.0" -FrontendImage "vue-search-app:v1.0.0"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$AcrName,
    
    [Parameter(Mandatory = $false)]
    [string]$ResourceGroup = "",
    
    [Parameter(Mandatory = $false)]
    [string]$BackendImage = "ase-enterprise-api",
    
    [Parameter(Mandatory = $false)]
    [string]$FrontendImage = "vue-search-app",
    
    [Parameter(Mandatory = $false)]
    [switch]$NoPush
)

# Append ':latest' when the caller omitted a tag
if ($BackendImage -notlike "*:*") { $BackendImage = "${BackendImage}:latest" }
if ($FrontendImage -notlike "*:*") { $FrontendImage = "${FrontendImage}:latest" }

$ErrorActionPreference = "Stop"

# Verify az CLI is available
if (-not (Get-Command "az" -ErrorAction SilentlyContinue)) {
    Write-Error "Azure CLI (az) is not installed. Install from: https://aka.ms/installazurecliwindows"
    exit 1
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir

Write-Host "🏗️  Azure Container Registry Build" -ForegroundColor Cyan
Write-Host "   Registry:  $AcrName.azurecr.io" -ForegroundColor Gray
Write-Host "   Backend:   $BackendImage" -ForegroundColor Gray
Write-Host "   Frontend:  $FrontendImage" -ForegroundColor Gray
Write-Host "   Note:      VITE_API_BASE_URL is set at container runtime, not baked in." -ForegroundColor DarkGray
Write-Host ""

# --- Build Backend ---
Write-Host "🔧 Building backend image..." -ForegroundColor Yellow
$backendContext    = Join-Path $repoRoot "src\AgentScratchEnterprise"
$backendDockerfile = Join-Path $backendContext "ASE.EnterpriseApi\Dockerfile"

$backendArgs = @(
    "acr", "build",
    "--registry", $AcrName,
    "--image", $BackendImage,
    "--file", $backendDockerfile
)
if ($ResourceGroup) { $backendArgs += @("--resource-group", $ResourceGroup) }
if ($NoPush) { $backendArgs += "--no-push" }
$backendArgs += $backendContext

Write-Host "   Context:    $backendContext" -ForegroundColor Gray
Write-Host "   Dockerfile: $backendDockerfile" -ForegroundColor Gray
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
$frontendContext    = Join-Path $repoRoot "src\chat-web-app"
$frontendDockerfile = Join-Path $frontendContext "Dockerfile"

$frontendArgs = @(
    "acr", "build",
    "--registry", $AcrName,
    "--image", $FrontendImage,
    "--file", $frontendDockerfile
)
if ($ResourceGroup) { $frontendArgs += @("--resource-group", $ResourceGroup) }
if ($NoPush) { $frontendArgs += "--no-push" }
$frontendArgs += $frontendContext

Write-Host "   Context:    $frontendContext" -ForegroundColor Gray
Write-Host "   Dockerfile: $frontendDockerfile" -ForegroundColor Gray
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
    Write-Host "   $AcrName.azurecr.io/$BackendImage" -ForegroundColor White
    Write-Host "   $AcrName.azurecr.io/$FrontendImage" -ForegroundColor White
}
