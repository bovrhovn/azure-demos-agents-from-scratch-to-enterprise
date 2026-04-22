<#
.SYNOPSIS
    Starts both the backend API and frontend Vue.js app in separate terminals.
.DESCRIPTION
    Launches ASE.EnterpriseApi (.NET 10) and Vue.js frontend (Vite) simultaneously.
    Press Ctrl+C in each window to stop, or close the windows.
#>

$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "🚀 Starting full stack: backend + frontend" -ForegroundColor Cyan
Write-Host ""

# Start backend in a new PowerShell window
$backendScript = Join-Path $scriptDir "run-backend.ps1"
Write-Host "🔧 Launching backend API..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-File", "`"$backendScript`""

# Give backend a moment to start
Start-Sleep -Seconds 2

# Start frontend in a new PowerShell window
$frontendScript = Join-Path $scriptDir "run-frontend.ps1"
Write-Host "⚛️  Launching frontend Vue.js app..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-File", "`"$frontendScript`""

Write-Host ""
Write-Host "✅ Both processes started in separate windows." -ForegroundColor Green
Write-Host "   Backend API:  http://localhost:5066" -ForegroundColor Green
Write-Host "   Frontend app: http://localhost:5173" -ForegroundColor Green
Write-Host ""
Write-Host "Close the launched windows to stop the services." -ForegroundColor Gray
