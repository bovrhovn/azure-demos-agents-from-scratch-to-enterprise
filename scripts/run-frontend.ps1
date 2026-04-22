$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
$webAppDir = Join-Path $repoRoot "src\chat-web-app"

Write-Host "⚛️  Starting Vue.js frontend..." -ForegroundColor Cyan
Write-Host "   App dir: $webAppDir" -ForegroundColor Gray
Write-Host "   URL:     http://localhost:5173" -ForegroundColor Green
Write-Host ""

Set-Location $webAppDir

if (-not (Test-Path "node_modules")) {
    Write-Host "📦 Installing dependencies..." -ForegroundColor Yellow
    npm install
}

npm run dev
