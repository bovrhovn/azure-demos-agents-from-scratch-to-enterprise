$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
$apiProject = Join-Path $repoRoot "src\AgentScratchEnterprise\ASE.EnterpriseApi\ASE.EnterpriseApi.csproj"

Write-Host "🔧 Starting ASE.EnterpriseApi backend..." -ForegroundColor Cyan
Write-Host "   Project: $apiProject" -ForegroundColor Gray
Write-Host "   API URL: http://localhost:5066" -ForegroundColor Green
Write-Host "   Environment: Development (uses DocumentSearchAdapter locally)" -ForegroundColor Gray
Write-Host ""
Write-Host "Press Ctrl+C to stop." -ForegroundColor Yellow
Write-Host ""

$env:ASPNETCORE_ENVIRONMENT = "Development"

try {
    dotnet run --project $apiProject --urls "http://localhost:5066"
} finally {
    Write-Host "" 
    Write-Host "✅ Backend stopped." -ForegroundColor Cyan
}
